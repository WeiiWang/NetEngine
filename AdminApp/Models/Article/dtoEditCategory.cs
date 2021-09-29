﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models.Article
{

    /// <summary>
    /// 创建栏目
    /// </summary>
    public class dtoEditCategory
    {


        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不可以空")]
        public string Name { get; set; }



        /// <summary>
        /// 频道ID
        /// </summary>
        public Guid ChannelId { get; set; }



        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentId { get; set; }




        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }



        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


    }
}
