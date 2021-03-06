﻿using Microsoft.AspNetCore.Http;

namespace Base
{
    /// <summary> 
    /// 创建人：落
    /// 日 期：2019-01-31 16:22:09
    /// 版 本：1.0
    /// 描 述：Ip扩展类
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 获取请求过来的IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
