using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace FileSaver
{
    public class Save:IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Saves single IFormFile to given directory. 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="directory">Default directory is wwwroot/content</param>
        /// <param name="size">Default size is 10MB</param>
        /// <returns></returns>
        public BaseMessage SingleFile(IFormFile formFile,string directory= "wwwroot/Content", int size = 10240 * 1024)
        {
            //string directory = "wwwroot/Content";
            Directory.CreateDirectory(directory);

            if (formFile != null)
            {
                string fileName, filePath, newFileName;

                if (formFile.Length > 0)
                {
                    if (formFile.Length <= size)
                    {

                        fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(Directory.GetCurrentDirectory(), directory) + $@"\{newFileName}";
                        filePath = directory + newFileName;
                        using (FileStream fs = File.Create(fileName))
                        {
                            formFile.CopyTo(fs);
                            fs.Flush();
                        }

                        return new BaseMessage { Status = true, Message = "Success", Data = filePath };

                    }
                    else
                    {
                        return new BaseMessage { Status = false, Message = $"File cannot be bigger than {size / (1024 * 1024)} mb", Data = null };

                    }
                }
                else
                {
                    return new BaseMessage { Status = false, Message = "File is corrupted", Data = null };
                }

            }
            return new BaseMessage { Status = false, Message = "File is empty", Data = null };

        }



        /// <summary>
        /// Saves multiple IFormFile to given directory. 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="directory">Default directory is wwwroot/content</param>
        /// <param name="size">Default size is 10MB</param>
        /// <returns></returns>
        public BaseMessage MultipleFiles(IFormFileCollection formFiles,string directory= "wwwroot/Content", int size = 4096 * 1024)
        {
            //string directory = "wwwroot/Content";
            Directory.CreateDirectory(directory);
            List<string> pathList = new List<string>();
            if (formFiles != null)
            {
                foreach (var formFile in formFiles)
                {


                    string fileName, filePath, newFileName;

                    if (formFile.Length > 0)
                    {
                        if (formFile.Length <= size)
                        {

                            fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            var FileExtension = Path.GetExtension(fileName);
                            newFileName = myUniqueFileName + FileExtension;
                            fileName = Path.Combine(Directory.GetCurrentDirectory(), directory) + $@"\{newFileName}";
                            filePath = directory + newFileName;
                            using (FileStream fs = File.Create(fileName))
                            {
                                formFile.CopyTo(fs);
                                fs.Flush();
                            }

                            pathList.Add(filePath);

                        }
                        else
                        {
                            return new BaseMessage { Status = false, Message = $"File cannot be bigger than {size / (1024 * 1024)} mb", Data = null };

                        }
                    }
                    else
                    {
                        return new BaseMessage { Status = false, Message = "File is corrupted", Data = null };
                    }
                }
                return new BaseMessage { Status = true, Message = "Success", Data = pathList };
            }

            return new BaseMessage { Status = false, Message = "File is empty", Data = null };



        }


       

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Save()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
