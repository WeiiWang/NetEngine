﻿using System;

namespace AdminShared.Models
{


    /// <summary>
    /// 文件信息
    /// </summary>
    public class DtoFileInfo
    {



        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }




        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }




        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }




        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get; set; }




        /// <summary>
        /// 显示文件大小
        /// </summary>
        public string DisplayLength { get; set; }




        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }



        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserId { get; set; }




        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }



    }
}
