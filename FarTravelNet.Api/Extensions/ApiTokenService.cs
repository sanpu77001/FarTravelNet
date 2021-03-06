﻿using FarTravelNet.Api.Utils;
using Microsoft.AspNetCore.Http;

namespace FarTravelNet.Api
{
    /// <summary> 
    /// 创建人：落
    /// 日 期：2019/6/15 21:16:23
    /// 版 本：1.0
    /// 描 述：API接口服务
    /// </summary>
    public interface IApiTokenService
    {
        /// <summary>
        /// 转换成token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string ConvertLoginToken(int userId, string userName);
        /// <summary>
        /// 根据token解密信息
        /// </summary>
        /// <returns></returns>
        UserApiToken GetUserPayloadByToken();
    }

    /// <summary>
    /// Token用户信息实体
    /// </summary>
    public class UserApiToken
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
    /// <summary>
    /// apiToken_唯一key
    /// </summary>
    public class ApiTokenConfig
    {
        /// <summary>
        /// 构造注入Key
        /// </summary>
        /// <param name="key"></param>
        public ApiTokenConfig(string key)
        {
            Api_Token_Key = key;
        }

        /// <summary>
        /// Key
        /// </summary>
        public string Api_Token_Key { get; set; }
    }

    /// <summary>
    /// API接口服务实现类
    /// </summary>
    public class ApiTokenService : IApiTokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _api_key_token;

        public ApiTokenService(ApiTokenConfig token
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _api_key_token = token.Api_Token_Key;
        }
        /// <summary>
        /// 获取登录token 
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public string ConvertLoginToken(int userId, string userName)
        {
            return JwtHelper.Encode(new UserApiToken() { UserId = userId, UserName = userName }, _api_key_token);
        }
        private UserApiToken _cachePaload;

        /// <summary>
        /// 获取登录信息 
        /// </summary>
        /// <returns></returns>
        public UserApiToken GetUserPayloadByToken()
        {
            if (_cachePaload != null)
                return _cachePaload;
            var token = _httpContextAccessor.HttpContext.Request.Headers["X-Token"];
            //header或者query带有x-token参数
            token = string.IsNullOrEmpty(token) ? _httpContextAccessor.HttpContext.Request.Query["x-token"] : token;
            if (string.IsNullOrEmpty(token))
                return null;
            _cachePaload = JwtHelper.Decode<UserApiToken>(token, _api_key_token);
            return _cachePaload;
        }
    }
}
