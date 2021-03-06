﻿using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün Eklendi";

        public static string ProductNameInvalid = "Ürün İsmi Geçersiz";
        public static string MaintenanceTime="Sistem Bakımda";
        public static string ProductsListed="Ürünler Listelendi";

        public static string ProductCountOfCategoryError = "Kategori Sınırı aşıldı";

        public static string ProductNameAllreadyExists = "Bu isimde başka bir kayıt var";

        public static string CheckIfCategoryLimitExceded = "Kategori limiti aşıldı";

        public static string AuthorizationDenied = "Yetkiniz yok";
    }
}
