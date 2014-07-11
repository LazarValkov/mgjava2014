using MgJava2014.Models;
using MgJava2014.Models.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MgJava2014.Controllers
{
    public class QuestionController : Controller
    {
        private MainDBEntities _dbConnection;
        private MainDBEntities dbConnection { get { return _dbConnection ?? (_dbConnection = new MainDBEntities()); } }
        
        //
        // GET: /Question/
        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var userId = dbConnection.Users.Single(user => user.username == username).Id;
            var answeredQuestions = dbConnection.AnsweredQuestions.Where(record => record.userId == userId).Select(record => record.questionId);
            var questions = dbConnection.Questions.Where(question => !answeredQuestions.Contains(question.Id));
            return View(questions);
        }

        [Authorize]
        public ActionResult Details(int questionId)
        {
            var username = User.Identity.Name;
            var userId = dbConnection.Users.Single(user => user.username == username).Id;

            var question = dbConnection.Questions.Single(aQuestion => aQuestion.Id == questionId);

            return View(question);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Submit(int questionId, HttpPostedFileBase file)
        {
            if (file != null)
            {
                try
                {
                    var username = User.Identity.Name;
                    var userId = dbConnection.Users.Single(user => user.username == username).Id;

                    MailMessage mail = new MailMessage();

                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    smtpServer.Credentials = new System.Net.NetworkCredential("lazarvalkov3@gmail.com", "javaclasses");
                    smtpServer.Port = 587; // Gmail works on this port

                    mail.From = new MailAddress("lazarvalkov3@gmail.com");
                    mail.To.Add("lazarvalkov2@gmail.com");
                    mail.Subject = "Question " + questionId + " submission by " + username;
                    mail.Body = "The user: " + username + " ,id: " + userId + " about questionId: " + questionId;

                    mail.Attachments.Add(new Attachment(file.InputStream, file.FileName, file.ContentType));


                    smtpServer.EnableSsl = true;
                    smtpServer.Send(mail);
                    ViewBag.SuccessMsg = "Файла беше изпратен успешно.";
                    return View();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = "Гррр. Възникна грешка при изпращането на файла. Бихте ли изпратили файла, потребителското си име и името на въпроса в email до lazarvalkov2@gmail.com?";
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Моля изберете файл.";
            }

            return View();
        }

        [Authorize(Users = "LazarValkov")]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize(Users = "LazarValkov")]
        [HttpPost]
        public ActionResult Add(Question model)
        {
            if (ModelState.IsValid || model.QuestionName != "" || model.QuestionText != "")
            {
                dbConnection.Questions.Add(model);
                dbConnection.SaveChanges();
                return RedirectToAction("Add");
            }
            return View();
        }

        [Authorize(Users = "LazarValkov")]
        public ActionResult QuestionAnswered()
        {
            return View();
        }

        [Authorize(Users = "LazarValkov")]
        [HttpPost]
        public ActionResult QuestionAnswered(QuestionAnswered model)
        {
            if (ModelState.IsValid)
            {
                var question = dbConnection.Questions.SingleOrDefault(q => q.QuestionName == model.questionName);
                var user = dbConnection.Users.SingleOrDefault(u => u.username == model.username);
                if (question == null)
                {
                    ViewBag.ErrorMsg = "Няма въпрос с такова име.";
                }
                else if (user == null)
                {
                    ViewBag.ErrorMsg = "Няма потребител с такова име.";
                }
                else
                {
                    bool alreadyAnswered = dbConnection.AnsweredQuestions.Any(aq => aq.questionId == question.Id && aq.userId == user.Id);
                    if (alreadyAnswered)
                    {
                        ViewBag.ErrorMsg = "Този потребител вече е отговорил на този въпрос.";
                    }
                    else
                    {
                        user.Score.points += question.Points;
                        dbConnection.AnsweredQuestions.Add(new AnsweredQuestion() { questionId = question.Id, userId = user.Id });
                        dbConnection.SaveChanges();
                        ViewBag.SuccessMsg = "Готово! Браво на човека.";
                    }
                }
            }
            return View();
        }

    }
}
