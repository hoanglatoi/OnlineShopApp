using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DapperDB.Models
{
    public partial class DbDataBase
    {
        #region column

        /// <summary>
		/// 最大件数
		/// </summary>
        /// 
        [Column("full_count")]
        [NotMapped]
        public virtual int full_count { get; set; }

        #endregion
    }
}
