﻿using Repository.Bases;
using System;

namespace Repository.Database
{
    public class TOSLog : CD
    {


        /// <summary>
        /// 外链表名
        /// </summary>
        public string Table { get; set; }



        /// <summary>
        /// 外链表ID
        /// </summary>
        public Guid TableId { get; set; }



        /// <summary>
        /// 标记
        /// </summary>
        public string Sign { get; set; }



        /// <summary>
        /// 变动内容
        /// </summary>
        public string Content { get; set; }



        /// <summary>
        /// 操作人信息
        /// </summary>
        public Guid? ActionUserId { get; set; }
        public virtual TUser ActionUser { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }



        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }



        /// <summary>
        ///  设备标记
        /// </summary>
        public string DeviceMark { get; set; }


    }
}
