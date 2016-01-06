using _NssForums2_Revengance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _NssForums2_Revengance.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using _NssForums2_Revengance.Controllers;

public class HomeController : ApplicationBaseController
{
    //public class ApplicationDbContext : DbContext
    //{
    //    public DbSet<Message> Messages { get; set; }
    //    public DbSet<Reply> Replies { get; set; }
    //}
    private ApplicationDbContext dbContext = new ApplicationDbContext();
    [Authorize]
    public ActionResult Index(int? Id, int? page)
    {
        int pageSize = 5;
        int pageNumber = (page ?? 1);
        MessageReplyViewModel vm = new MessageReplyViewModel();
        var count = dbContext.Messages.Count();

        decimal totalPages = count / (decimal)pageSize;
        ViewBag.TotalPages = Math.Ceiling(totalPages);
        vm.Messages = dbContext.Messages
                                   .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
        ViewBag.MessagesInOnePage = vm.Messages;
        ViewBag.PageNumber = pageNumber;

        if (Id != null)
        {

            var replies = dbContext.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
            if (replies != null)
            {
                foreach (var rep in replies)
                {
                    MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                    reply.MessageId = rep.MessageId;
                    reply.Id = rep.Id;
                    reply.ReplyMessage = rep.ReplyMessage;
                    reply.ReplyDateTime = rep.ReplyDateTime;
                    reply.MessageDetails = dbContext.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                    reply.ReplyFrom = rep.ReplyFrom;
                    vm.Replies.Add(reply);
                }

            }
            else
            {
                vm.Replies.Add(null);
            }


            ViewBag.MessageId = Id.Value;
        }

        return View(vm);
    }

}
