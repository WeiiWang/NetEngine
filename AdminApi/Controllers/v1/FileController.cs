﻿using AdminApi.Libraries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdminApi.Controllers.v1
{

    /// <summary>
    /// 文件上传控制器
    /// </summary>
    [ApiVersion("1")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerCore
    {



        /// <summary>
        /// 远程单文件上传接口
        /// </summary>
        /// <param name="business">业务领域</param>
        /// <param name="key">记录值</param>
        /// <param name="sign">自定义标记</param>
        /// <param name="fileInfo">Key为文件URL,Value为文件名称</param>
        /// <returns>文件ID</returns>
        [HttpPost("RemoteUploadFile")]
        public Guid RemoteUploadFile([FromQuery][Required] string business, [FromQuery][Required] Guid key, [FromQuery][Required] string sign, [Required][FromBody] dtoKeyValue fileInfo)
        {
            string remoteFileUrl = fileInfo.Key.ToString();

            var fileExtension = Path.GetExtension(fileInfo.Value.ToString()).ToLower();
            var fileName = Guid.NewGuid().ToString() + fileExtension;

            string basepath = "/Upload/" + DateTime.Now.ToString("yyyy/MM/dd");

            var filePath = Libraries.IO.Path.WebRootPath() + basepath + "/";

            //下载文件
            var dlPath = Common.IO.IOHelper.DownloadFile(remoteFileUrl, filePath, fileName);

            if (dlPath == null)
            {
                Thread.Sleep(5000);
                dlPath = Common.IO.IOHelper.DownloadFile(remoteFileUrl, filePath, fileName);
            }

            filePath = dlPath.Replace(Libraries.IO.Path.ContentRootPath(), "");


            if (dlPath != null)
            {
                var isSuccess = true;

                var upRemote = false;

                if (upRemote == true)
                {
                    var oss = new Common.AliYun.OssHelper();

                    var upload = oss.FileUpload(dlPath, basepath, fileInfo.Value.ToString());

                    if (upload)
                    {
                        Common.IO.IOHelper.DeleteFile(dlPath);
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }

                if (isSuccess)
                {

                    var f = new TFile();
                    f.Id = Guid.NewGuid();
                    f.Name = fileInfo.Value.ToString();
                    f.Path = filePath;
                    f.Table = business;
                    f.TableId = key;
                    f.Sign = sign;
                    f.CreateUserId = userId;
                    f.CreateTime = DateTime.Now;
                    db.TFile.Add(f);
                    db.SaveChanges();

                    return f.Id;
                }

            }

            HttpContext.Response.StatusCode = 400;
            HttpContext.Items.Add("errMsg", "文件上传失败！");

            return default;
        }



        /// <summary>
        /// 单文件上传接口
        /// </summary>
        /// <param name="business">业务领域</param>
        /// <param name="key">记录值</param>
        /// <param name="sign">自定义标记</param>
        /// <param name="file">file</param>
        /// <returns>文件ID</returns>
        [DisableRequestSizeLimit]
        [HttpPost("UploadFile")]
        public Guid UploadFile([FromQuery][Required] string business, [FromQuery][Required] Guid key, [FromQuery][Required] string sign, [Required] IFormFile file)
        {

            string basepath = "/Upload/" + DateTime.Now.ToString("yyyy/MM/dd");
            string filepath = Libraries.IO.Path.WebRootPath() + basepath;

            Directory.CreateDirectory(filepath);

            var fileName = Guid.NewGuid();
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fullFileName = string.Format("{0}{1}", fileName, fileExtension);

            string path = "";

            var isSuccess = false;

            if (file != null && file.Length > 0)
            {
                path = filepath + "/" + fullFileName;

                using (FileStream fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                var upRemote = false;

                if (upRemote)
                {

                    var oss = new Common.AliYun.OssHelper();

                    var upload = oss.FileUpload(path, "Upload/" + DateTime.Now.ToString("yyyy/MM/dd"), file.FileName);

                    if (upload)
                    {
                        Common.IO.IOHelper.DeleteFile(path);

                        path = "/Upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + fullFileName;
                        isSuccess = true;
                    }
                }
                else
                {
                    path = basepath + "/" + fullFileName;
                    isSuccess = true;
                }

            }

            if (isSuccess)
            {

                var f = new TFile();
                f.Id = fileName;
                f.IsDelete = false;
                f.Name = file.FileName;
                f.Path = path;
                f.Table = business;
                f.TableId = key;
                f.Sign = sign;
                f.CreateUserId = userId;
                f.CreateTime = DateTime.Now;
                db.TFile.Add(f);
                db.SaveChanges();

                return fileName;
            }
            else
            {
                HttpContext.Response.StatusCode = 400;

                HttpContext.Items.Add("errMsg", "文件上传失败！");

                return default;
            }
        }



        /// <summary>
        /// 多文件上传接口
        /// </summary>
        /// <param name="business">业务领域</param>
        /// <param name="key">记录值</param>
        /// <param name="sign">标记</param>
        /// <returns></returns>
        /// <remarks>swagger 暂不支持多文件接口测试，请使用 postman</remarks>
        [DisableRequestSizeLimit]
        [HttpPost("BatchUploadFile")]
        public List<Guid> BatchUploadFile([FromQuery][Required] string business, [FromQuery][Required] Guid key, [FromQuery][Required] string sign)
        {
            var fileIds = new List<Guid>();

            var ReqFiles = Request.Form.Files;


            List<IFormFile> Attachments = new List<IFormFile>();
            for (int i = 0; i < ReqFiles.Count; i++)
            {
                Attachments.Add(ReqFiles[i]);
            }

            foreach (var file in Attachments)
            {
                var fileId = UploadFile(business, key, sign, file);

                fileIds.Add(fileId);
            }

            return fileIds;
        }



        /// <summary>
        /// 通过文件ID获取文件
        /// </summary>
        /// <param name="fileid">文件ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetFile")]
        public FileResult GetFile([Required] Guid fileid)
        {

            var file = db.TFile.Where(t => t.Id == fileid).FirstOrDefault();
            string path = Libraries.IO.Path.WebRootPath() + file.Path;


            //读取文件入流
            var stream = System.IO.File.OpenRead(path);

            //获取文件后缀
            string fileExt = Path.GetExtension(path);

            //获取系统常规全部mime类型
            var provider = new FileExtensionContentTypeProvider();

            //通过文件后缀寻找对呀的mime类型
            var memi = provider.Mappings.ContainsKey(fileExt) ? provider.Mappings[fileExt] : provider.Mappings[".zip"];


            return File(stream, memi, file.Name);
        }




        /// <summary>
        /// 通过图片文件ID获取图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        /// <remarks>不指定宽高参数,返回原图</remarks>
        [AllowAnonymous]
        [HttpGet("GetImage")]
        public FileResult GetImage([Required] Guid fileId, int width, int height)
        {

            var file = db.TFile.Where(t => t.Id == fileId).FirstOrDefault();
            var path = Libraries.IO.Path.WebRootPath() + file.Path;

            var stream = System.IO.File.OpenRead(path);

            string fileExt = Path.GetExtension(path);

            var provider = new FileExtensionContentTypeProvider();

            var memi = provider.Mappings[fileExt];

            if (width == 0 && height == 0)
            {
                return File(stream, memi, file.Name);
            }
            else
            {
                Image img = Image.FromStream(stream);

                if (Array.IndexOf(img.PropertyIdList, 274) > -1)
                {
                    var orientation = 0;

                    var platform = Environment.OSVersion.Platform;

                    if (platform == PlatformID.Win32NT)
                    {
                        orientation = (int)img.GetPropertyItem(274).Value[0];
                    }

                    if (platform == PlatformID.Unix)
                    {
                        orientation = (int)img.GetPropertyItem(274).Value[1];
                    }


                    switch (orientation)
                    {
                        case 1:
                            // No rotation required.
                            break;
                        case 2:
                            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 3:
                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            img.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case 5:
                            img.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case 6:
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            img.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case 8:
                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    img.RemovePropertyItem(274);
                }

                if (img.Width < width || img.Height < height)
                {
                    img.Dispose();

                    stream = System.IO.File.OpenRead(path);

                    return File(stream, memi, file.Name);
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        if (width != 0 && height == 0)
                        {
                            var percent = ((float)width / (float)img.Width);

                            width = (int)(img.Width * percent);
                            height = (int)(img.Height * percent);
                        }

                        if (width == 0 && height != 0)
                        {
                            var percent = ((float)height / (float)img.Height);

                            width = (int)(img.Width * percent);
                            height = (int)(img.Height * percent);
                        }

                        img.GetThumbnailImage(width, height, null, IntPtr.Zero).Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        img.Dispose();
                        ms.Dispose();

                        return File(ms.ToArray(), "image/png");
                    }
                }

            }
        }



        /// <summary>
        /// 通过文件ID获取文件静态访问路径
        /// </summary>
        /// <param name="fileid">文件ID</param>
        /// <returns></returns>
        [HttpGet("GetFilePath")]
        public string GetFilePath([Required] Guid fileid)
        {

            var file = db.TFile.AsNoTracking().Where(t => t.IsDelete == false & t.Id == fileid).FirstOrDefault();

            if (file != null)
            {
                string domain = "https://file.xxxx.com";

                string fileUrl = domain + file.Path.Replace("\\", "/");

                return fileUrl;
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Items.Add("errMsg", "通过指定的文件ID未找到任何文件！");

                return null;
            }

        }




        /// <summary>
        /// 通过文件ID删除文件方法
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        [HttpDelete("DeleteFile")]
        public bool DeleteFile(dtoId id)
        {

            var file = db.TFile.Where(t => t.IsDelete == false && t.Id == id.Id).FirstOrDefault();

            file.IsDelete = true;
            file.DeleteTime = DateTime.Now;

            db.SaveChanges();

            return true;

        }


    }
}