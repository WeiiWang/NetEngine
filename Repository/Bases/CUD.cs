﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Bases
{

    /// <summary>
    /// 创建，编辑，删除
    /// </summary>
    public class CUD:CD
    {


        /// <summary>
        /// 更新时间
        /// </summary>
        [ConcurrencyCheck]
        public DateTime? UpdateTime { get; set; }

    }
}
