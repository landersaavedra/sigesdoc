using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public static class DropDownListExtensions
{
    public static IEnumerable<SelectListItem> GetDropDownList<T>(IEnumerable<T> lista,
       string text, string value, string selected = "", string textDefault = "Todos") where T : class
    {
        List<SelectListItem> list = new List<SelectListItem>();
        list.Add(new SelectListItem { Text = textDefault, Value = string.Empty });
        var lisData = (from items in lista
                       select items).AsEnumerable().Select(m => new SelectListItem
                       {
                           Text = m.GetType().GetProperty(text).GetValue(m).ToString(),
                           Value = m.GetType().GetProperty(value).GetValue(m).ToString(),
                           Selected = (selected != "") ? (m.GetType().GetProperty(value).GetValue(m).ToString() ==
                             selected ? true : false) : false,
                       }).ToList();
        list.AddRange(lisData);
        return list.AsEnumerable();
    }

}