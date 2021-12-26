using Library.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.API.Controllers
{
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var links = new List<Link>();
            links.Add(new Link(HttpMethods.Get,
              "self",
              Url.Link(nameof(GetRoot), null)));

            links.Add(new Link(HttpMethods.Get,
                "get authors",
                Url.Link(nameof(AuthorController.GetAuthorsAsync), null)));

            links.Add(new Link(HttpMethods.Post,
                "create author",
                Url.Link(nameof(AuthorController.CreateAuthorAsync), null)));

            links.Add(new Link(HttpMethods.Post,
                "add user",
                Url.Link(nameof(AuthenticateController.AddUserAsync), null)));

            links.Add(new Link(HttpMethods.Post,
                "get token",
                Url.Link(nameof(AuthenticateController.GenerateTokenAsync), null)));

            return Ok(links);

            #region 获取控制器
            //List<Type> controllerTypes = new List<Type>();

            ////加载程序集
            //var assembly = Assembly.Load("Library.API");

            ////获取程序集下所有的类，通过Linq筛选继承IController类的所有类型
            //controllerTypes.AddRange(assembly.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Name != "ErrorController"));

            ////创建动态字符串，拼接json数据    注：现在json类型传递数据比较流行，比xml简洁
            //StringBuilder jsonBuilder = new StringBuilder();
            //jsonBuilder.Append("[");

            ////遍历控制器类
            //foreach (var controller in controllerTypes)
            //{
            //    jsonBuilder.Append("{\"controllerName\":\"");
            //    jsonBuilder.Append(controller.Name);
            //    jsonBuilder.Append("\",\"controllerDesc\":\"");
            //    jsonBuilder.Append((controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute) == null ? "" : (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description);



            //    //获取控制器下所有返回类型为ActionResult的方法，对MVC的权限控制只要限制所以的前后台交互请求就行，统一为ActionResult

            //    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            //    jsonBuilder.Append("]},");
            //}
            //jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            //jsonBuilder.Append("]");
            //return Content(jsonBuilder.ToString());
            #endregion
        }
    }
}