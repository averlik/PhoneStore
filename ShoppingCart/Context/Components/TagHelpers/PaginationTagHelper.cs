using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace PhoneStore.Context.Components.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageRange { get; set; }
        public string PageFirst { get; set; }
        public string PageLast { get; set; }
        public string PageTarget { get; set; }
        public string AdditionalQueryParameters { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("aria-label", "Page navigation");
            output.Content.SetHtmlContent(AddPageContent());
        }

        private string AddPageContent()
        {
            if (PageRange == 0)
            {
                PageRange = 1;
            }

            if (PageCount < PageRange)
            {
                PageRange = PageCount;
            }

            if (string.IsNullOrEmpty(PageFirst))
            {
                PageFirst = "Первая";
            }

            if (string.IsNullOrEmpty(PageLast))
            {
                PageLast = "Последняя";
            }

            var content = new StringBuilder();
            content.Append(" <ul class='pagination'>");

            // Добавить первую страницу
            if (PageNumber != 1)
            {
                content.Append($"<li class='page-item'><a class='page-link' href='{PageTarget}?p=1{AdditionalQueryParameters}'>{PageFirst}</a></li>");
            }

            // Генерация номеров страниц
            for (int currentPage = 1; currentPage <= PageCount; currentPage++)
            {
                var active = currentPage == PageNumber ? "active" : "";
                content.Append($"<li class='page-item {active}'><a class='page-link' href='{PageTarget}?p={currentPage}{AdditionalQueryParameters}'>{currentPage}</a></li>");
            }

            // Добавить последнюю страницу
            if (PageNumber != PageCount)
            {
                content.Append($"<li class='page-item'><a class='page-link' href='{PageTarget}?p={PageCount}{AdditionalQueryParameters}'>{PageLast}</a></li>");
            }

            content.Append(" </ul>");
            return content.ToString();
        }
    }
}