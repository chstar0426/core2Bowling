using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace core2Bowling.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var menuList = new List<MainMenu>
            {

               new MainMenu {controllerName = "Games", actionName = "Index", linkText = "경기관리", level = 1, icon = "glyphicon glyphicon-th-list"}
            };

            if ((string)ViewContext.RouteData.Values["controller"] == "TeamMembers")
            {
                menuList.Add(new MainMenu { controllerName = "TeamMembers", actionName = "Index", linkText = "경기내용", level = 1, icon = "glyphicon glyphicon-blackboard" });
                
            }
            menuList.Add(new MainMenu { controllerName = "RecordStatics", actionName = "CalPenaltyAvg", linkText = "벌금에버관리", level = 0, icon = "glyphicon glyphicon-pencil" });
            menuList.Add(new MainMenu { controllerName = "RecordStatics", actionName = "CalYearAvg", linkText = "에버관리", level = 0, icon = "glyphicon glyphicon-paperclip" });

            //new MainMenu {controllerName = "Home", actionName = "start", linkText = "풀다운메뉴 방식", icon = "fa fa-dashboard fa-fw", level = 1 },
            //new MainMenu {controllerName = "Home", actionName = "Index", linkText = "홈", icon = "", level = 2 },
            //new MainMenu {controllerName = "Home", actionName = "About", linkText = "대하여", icon = "", level = 2 },
            //new MainMenu {controllerName = "Home", actionName = "Contact", linkText = "도움센터", icon = "", level = 2 },
            //new MainMenu {controllerName = "Home", actionName = "end", linkText = "", icon = "",level = 1 },

            if (User.IsInRole("Admin"))
            {
                menuList.Add(new MainMenu { controllerName = "Bowlers", actionName = "Index", linkText = "회원관리", level = 1, icon = "glyphicon glyphicon-user" });
                menuList.Add(new MainMenu { controllerName = "YearAverages", actionName = "Index", linkText = "연도별관리", level = 0, icon = "glyphicon glyphicon-calendar" });

            }

            return View(menuList);

        }

    }
}
