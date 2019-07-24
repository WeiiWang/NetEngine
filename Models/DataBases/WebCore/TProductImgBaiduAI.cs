﻿using Models.DataBases.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DataBases.WebCore
{

    /// <summary>
    /// 产品图片对应百度AI信息表
    /// </summary>
    [Table("t_product_img_baiduai")]
    public class TProductImgBaiduAi : CUD
    {


        /// <summary>
        /// 产品图片ID
        /// </summary>
        public string ProductImgId { get; set; }
        public TProductImg ProductImg { get; set; }


        /// <summary>
        /// 图片库唯一标识符
        /// </summary>
        public string Unique { get; set; }


        /// <summary>
        /// 接口返回值
        /// </summary>
        public string Result { get; set; }

    }
}
