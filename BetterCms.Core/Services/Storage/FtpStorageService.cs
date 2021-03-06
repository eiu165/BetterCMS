﻿using System;
using System.IO;
using System.Net;

using BetterCms.Core.Exceptions.Service;

namespace BetterCms.Core.Services.Storage
{
    public class FtpStorageService : IStorageService
    {
        private readonly string rootUrl;
        private readonly string userName;
        private readonly string password;
        private readonly string ftpRoot;
        private readonly bool usePassiveMode;

        public FtpStorageService(ICmsConfiguration config)
        {
            try
            {
                var serviceSection = config.Storage;
                var mode = serviceSection.GetValue("UsePassiveMode");

                rootUrl = config.Storage.ContentRoot;
                usePassiveMode = mode != null && bool.Parse(mode);
                ftpRoot = serviceSection.GetValue("FtpRoot");
                userName = serviceSection.GetValue("FtpUserName");
                password = serviceSection.GetValue("FtpPassword");

                if (string.IsNullOrEmpty(rootUrl))
                {
                    throw new StorageException("ContentRoot is missing in a storage configuration.");
                }

                rootUrl = rootUrl.TrimEnd('/');
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to initialize storage service {0}.", GetType()), e);
            }
        }

        public bool ObjectExists(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var absolutePath = ResolvePath(uri.LocalPath);
                var serverUri = string.Format("{0}{1}", ftpRoot, absolutePath);
                var ftpRequest = CreateFtpRequest(serverUri);
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                try
                {
                    var response = (FtpWebResponse)ftpRequest.GetResponse();
                    response.Close();
                    return true;
                }
                catch (WebException ex)
                {
                    var response = (FtpWebResponse)ex.Response;
                    var statusCode = response.StatusCode;
                    response.Close();
                    if (statusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                    throw;
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to check if object exists under {0}.", uri), e);
            }
        }

        public void UploadObject(UploadRequest request)
        {
            CheckUri(request.Uri);

            try
            {
                var absolutePath = ResolvePath(request.Uri.AbsoluteUri);

                if (request.CreateDirectory)
                {
                    var ftpDiretoryUri = ExtractPath(absolutePath);
                    TryCreateDirectory(ftpDiretoryUri);
                }

                request.InputStream.Position = 0;

                var serverUri = string.Format("{0}{1}", ftpRoot, absolutePath);
                var ftpRequest = CreateFtpRequest(serverUri);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                using (var requestSream = ftpRequest.GetRequestStream())
                {
                    Pump(request.InputStream, requestSream);
                }

                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to upload object with request {0}.", request), e);
            }
        }

        /// <summary>
        /// Downloads the object.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns><c>DownloadResponse</c> response.</returns>
        /// <exception cref="StorageException">if file downloading failed.</exception>
        public DownloadResponse DownloadObject(Uri uri)
        {
            CheckUri(uri);

            try
            {
                var ftpRequest = CreateFtpRequest(uri.ToString());
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                var response = ftpRequest.GetResponse();
                var downloadResponse = new DownloadResponse { Uri = uri };

                using (var responseStream = response.GetResponseStream())
                {
                    downloadResponse.ResponseStream = new MemoryStream();
                    responseStream.CopyTo(downloadResponse.ResponseStream);
                }

                return downloadResponse;
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to download object from {0}.", uri), e);
            }
        }

        public void CopyObject(Uri sourceUri, Uri destinationUri)
        {
            CheckUri(sourceUri);
            CheckUri(destinationUri);

            try
            {
                var response = DownloadObject(sourceUri);

                UploadRequest request = new UploadRequest();
                request.Uri = destinationUri;
                request.InputStream = new MemoryStream();

                response.ResponseStream.Position = 0;
                response.ResponseStream.CopyTo(request.InputStream);

                UploadObject(request);
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to copy object. SourceUrl: {0}, DestinationUrl: {1}", sourceUri, destinationUri), e);
            }
        }

        public void RemoveObject(Uri uri)
        {
            CheckUri(uri);

            var absolutePath = ResolvePath(uri.LocalPath);
            var serverUri = string.Format("{0}{1}", ftpRoot, absolutePath);
            FtpWebRequest request = CreateFtpRequest(serverUri);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();            
            response.Close();            
        }

        public void RemoveFolder(Uri uri)
        {
            CheckUri(uri);

            var absolutePath = ResolvePath(Path.GetDirectoryName(uri.LocalPath));
            var serverUri = string.Format("{0}{1}", ftpRoot, absolutePath);
            FtpWebRequest request = CreateFtpRequest(serverUri);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        private void CheckUri(Uri uri)
        {
            if (!Uri.CheckSchemeName(uri.Scheme) || !uri.Scheme.Equals(Uri.UriSchemeFtp))
            {
                throw new StorageException(string.Format("An Uri scheme {0} is invalid. Uri {1} can't be processed with a {2} storage service.", uri.Scheme, uri, GetType().Name));
            }
        } 

        private static void Pump(Stream input, Stream output)
        {
            var buffer = new byte[2048];
            while (true)
            {
                var bytesRead = input.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                output.Write(buffer, 0, bytesRead);
            }
        }

        private string ResolvePath(string url)
        {
            if (!url.StartsWith(rootUrl, StringComparison.OrdinalIgnoreCase))
            {
                throw new StorageException(string.Format("RootUrl [{0}] must match for the url [{1}]", rootUrl, url));
            }

            return url.Substring(rootUrl.Length);
        }

        public void CreateDirectory(string serverUri)
        {
            try
            {
                var ftpRequest = CreateFtpRequest(serverUri);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to create directory: {0}", serverUri), e);
            }
        }

        private bool DirectoryExists(string serverUri)
        {
            try
            {
                try
                {
                    var request = CreateFtpRequest(serverUri);
                    request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                    var response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                    return true;
                }
                catch (WebException)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new StorageException(string.Format("Failed to check if directory {0} exists.", serverUri), e);
            }
        }

        private string ExtractPath(string absolutePath)
        {
            const char TestChar = '/';
            var lastIndexOfSlash = absolutePath.LastIndexOf(TestChar);
            return ftpRoot + absolutePath.Substring(0, lastIndexOfSlash).TrimEnd(TestChar);
        }

        private void TryCreateDirectory(string sereverUri)
        {
            try
            {
                CreateDirectory(sereverUri);
            }
            catch (Exception)
            {
                if (!DirectoryExists(sereverUri))
                {
                    throw;
                }
            }
        }

        private FtpWebRequest CreateFtpRequest(string serverUri)
        {
            var ftpRequest = (FtpWebRequest)WebRequest.Create(serverUri);
            ftpRequest.Credentials = new NetworkCredential(userName, password);
            ftpRequest.UsePassive = usePassiveMode;
            ftpRequest.UseBinary = true;
            ftpRequest.KeepAlive = false;

            return ftpRequest;
        }
    }
}
