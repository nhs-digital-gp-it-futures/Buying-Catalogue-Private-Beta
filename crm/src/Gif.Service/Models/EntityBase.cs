#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Gif.Service.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Gif.Service.Models
{
  [DataContract]
  public class EntityBase
  {
    private const string regexIdMatch = "";
    private const string jsonSeparator = "\":\"";
    private const string doubleQuote = "\"";
    private const char colon = ':';
    private const char openParenthesis = '(';
    private const char closeParenthesis = ')';
    private const char openBracket = '{';
    private const char closeBracket = '}';
    private const char forwardSlash = '/';
    private const char comma = ',';

    #region Constructor

    public EntityBase() { }

    public EntityBase(JToken record)
    {
      this.ParseJson(record);
    }

    #endregion

    #region common properties

    [DataMember]
    [CrmFieldName("createdon")]
    public DateTime? CreatedOn { get; set; }

    [DataMember]
    [CrmFieldName("modifiedon")]
    public DateTime? ModifiedOn { get; set; }

    #endregion

    /// <summary>
    /// Populate this entity with the values from a Json token object
    /// </summary>
    /// <remarks>
    /// Unlike JsonConvert.Deserialize this method matches on CrmFieldName Attributes
    /// </remarks>
    /// <param name="record">The json string to parse</param>
    public void ParseJson(JToken record)
    {
      if (record == null)
      {
        throw new ArgumentNullException();
      }

      foreach (var p in GetType().GetProperties())
      {
        var field = p.GetCustomAttribute<CrmFieldNameAttribute>()?.Name;
        var key = p.GetCustomAttribute<CrmEntityRelationAttribute>()?.Name;

        if (field != null)
        {
          p.SetValue(this, record[field].ToObject(p.PropertyType));
          continue;
        }

        if (key == null || record[key] == null)
          continue;

        if (!p.PropertyType.FullName.Contains("System.Collections.Generic.IList"))
          p.SetValue(this, record[key].ToObject(p.PropertyType));

      }
    }

    /// <summary>
    /// Serialise the entity to json using its odata field names
    /// </summary>
    /// <param name="idName">Exclude all fields with the CrmFieldId attribute</param>
    /// <returns>This object serialised to json</returns>
    public string SerializeToODataPut(string idName)
    {
      string dataString = JsonConvert.SerializeObject(this);

      var entityName = GetType().GetCustomAttribute<CrmEntityAttribute>()?.Name;
      if (string.IsNullOrEmpty(entityName))
        return dataString;

      foreach (var p in GetType().GetProperties())
      {
        if (!p.PropertyType.FullName.Contains("System.Collections.Generic.IList"))
        {
          var targetFieldDataBind = p.GetCustomAttribute<CrmFieldNameDataBindAttribute>()?.Name;

          if (!string.IsNullOrEmpty(targetFieldDataBind) && targetFieldDataBind.EndsWith("odata.bind"))
          {
            var entityNameBind = p.GetCustomAttribute<CrmFieldEntityDataBindAttribute>()?.Name;
            dataString = ReplaceDataBind(entityNameBind, targetFieldDataBind, dataString, p.Name);
            continue;
          }

          var targetField = p.GetCustomAttribute<CrmFieldNameAttribute>()?.Name;

          if (!string.IsNullOrEmpty(targetField) && targetField == idName)
          {
            dataString = ReplaceId(dataString, p.Name);
            continue;
          }

          if (targetField == null)
          {
            dataString = Regex.Replace(dataString, @",?\""" + p.Name + @"\"":null", "");
            continue;
          }

          if (p.Name.ToLower() == "createdon" || p.Name.ToLower() == "modifiedon")
          {
            dataString = ReplaceDataBind(entityName, targetField, dataString, p.Name);
            continue;
          }

          if (p.PropertyType.FullName.ToLower().Contains("system.string"))
          {
            dataString = ReplaceNullString(dataString, p.Name);
          }

          if (p.PropertyType.FullName.ToLower().Contains("system.datetime"))
          {
            dataString = ReplaceNullDate(dataString, p.Name);
          }

          dataString = dataString.Replace("\"" + p.Name + "\"", "\"" + targetField + "\"");
        }
      }

      dataString = RemoveExtraneousCharacters(dataString);
      return dataString;
    }

    private string ReplaceId(string dataString, string name)
    {
      var startEntity = dataString.IndexOf(name, 0, StringComparison.Ordinal);
      var startGuid = startEntity + name.Length + jsonSeparator.Length;
      var endGuid = dataString.IndexOf(doubleQuote, startGuid + 1, StringComparison.Ordinal);
      var entityVal = dataString.Substring(startGuid, endGuid - startGuid);

      var replace = $"{name}{jsonSeparator}{entityVal}";
      dataString = dataString.Replace(replace, string.Empty);

      return dataString;
    }

    /// <summary>
    /// Serialise the entity to json using its odata field names
    /// </summary>
    /// <param name="excludeId">Exclude all fields with the CrmFieldId attribute</param>
    /// <returns>This object serialised to json</returns>
    public string SerializeToODataPost(bool excludeId = false)
    {
      var dataString = JsonConvert.SerializeObject(this);

      var entityName = GetType().GetCustomAttribute<CrmEntityAttribute>()?.Name;
      if (string.IsNullOrEmpty(entityName))
        return dataString;

      foreach (var p in GetType().GetProperties())
      {
        var targetFieldDataBind = p.GetCustomAttribute<CrmFieldNameDataBindAttribute>()?.Name;

        if (!string.IsNullOrEmpty(targetFieldDataBind) && targetFieldDataBind.EndsWith("odata.bind"))
        {
          var entityNameBind = p.GetCustomAttribute<CrmFieldEntityDataBindAttribute>()?.Name;
          dataString = ReplaceDataBind(entityNameBind, targetFieldDataBind, dataString, p.Name);
          continue;
        }

        var targetField = p.GetCustomAttribute<CrmFieldNameAttribute>()?.Name;

        if (targetField == null)
        {
          dataString = Regex.Replace(dataString, @",?\""" + p.Name + @"\"":null", "");
          continue;
        }

        if (p.Name.ToLower() == "createdon" || p.Name.ToLower() == "modifiedon")
        {
          dataString = ReplaceDataBind(entityName, targetField, dataString, p.Name);
          continue;
        }

        if (p.PropertyType.FullName.ToLower().Contains("system.string"))
        {
          dataString = ReplaceNullString(dataString, p.Name);
        }

        if (p.PropertyType.FullName.ToLower().Contains("system.datetime"))
        {
          dataString = ReplaceNullDate(dataString, p.Name);
        }

        dataString = dataString.Replace("\"" + p.Name + "\"", "\"" + targetField + "\"");

        if (p.GetCustomAttribute<CrmIdFieldAttribute>() != null && excludeId)
          dataString = Regex.Replace(dataString, @"\""" + targetField + @"\"":\""0+\-0+\-0+\-0+\-0+\"",", "");
      }

      dataString = RemoveExtraneousCharacters(dataString);
      return dataString;
    }

    private string RemoveExtraneousCharacters(string dataString)
    {
      dataString = RemoveDoubleCharacters(dataString, $"{comma}");
      dataString = dataString.Replace($"{comma}{closeBracket}", $"{closeBracket}");
      dataString = dataString.Replace($"{openBracket}{doubleQuote}{doubleQuote}{comma}", $"{openBracket}");
      dataString = dataString.Replace(",\"\"", string.Empty);
      return dataString;
    }

    private string RemoveDoubleCharacters(string dataString, string character)
    {
      while (true)
      {
        if (dataString.IndexOf($"{character}{character}", StringComparison.Ordinal) == -1)
          return dataString;
        dataString = dataString.Replace($"{character}{character}", $"{character}");
      }
    }

    private string ReplaceDataBind(string entityName, string targetField, string dataString, string name)
    {
      string entityVal = "null";
      var startEntity = dataString.IndexOf($"{name}{doubleQuote}{colon}{entityVal}", 0, StringComparison.Ordinal);
      var replace = $"{doubleQuote}{name}{doubleQuote}{colon}null";

      if (startEntity == -1)
      {
        startEntity = dataString.IndexOf($"{name}{doubleQuote}", 0, StringComparison.Ordinal);
        var startGuid = startEntity + name.Length + jsonSeparator.Length;
        var endGuid = dataString.IndexOf(doubleQuote, startGuid + 1, StringComparison.Ordinal);
        entityVal = dataString.Substring(startGuid, endGuid - startGuid);
        replace = $"{name}{jsonSeparator}{entityVal}";
      }

      var newFormat = $"{targetField}{jsonSeparator}{forwardSlash}{entityName.ToLower()}{openParenthesis}{entityVal}{closeParenthesis}";

      dataString = ReplaceLookups(dataString, entityVal, replace, newFormat);

      return dataString;
    }

    private string ReplaceNullString(string dataString, string name)
    {
      string entityVal = "null";
      var startEntity = dataString.IndexOf($"{name}{doubleQuote}{colon}{entityVal}", 0, StringComparison.Ordinal);

      if (startEntity != -1)
      {
        var replace = $"{doubleQuote}{name}{doubleQuote}{colon}null";
        dataString = dataString.Replace(replace, string.Empty);
      }

      return dataString;
    }

    private string ReplaceNullDate(string dataString, string name)
    {
      string entityVal = "0001-01-01T00:00:00";
      var startEntity = dataString.IndexOf($"{name}{doubleQuote}{colon}{doubleQuote}{entityVal}", 0, StringComparison.Ordinal);

      if (startEntity != -1)
      {
        var replace = $"{doubleQuote}{name}{doubleQuote}{colon}{doubleQuote}0001-01-01T00:00:00{doubleQuote}";
        dataString = dataString.Replace(replace, string.Empty);
      }

      return dataString;
    }

    private string ReplaceLookups(string dataString, string entityVal, string replace, string newFormat)
    {
      Guid outGuid = Guid.Empty;
      Guid.TryParse(entityVal, out outGuid);

      dataString = dataString.Replace(replace, outGuid != Guid.Empty ? newFormat : String.Empty);
      return dataString;
    }

    /// <summary>
    /// Build OData query string
    /// </summary>
    /// <remarks>
    /// String is built from Entity declaration, therefore type argument must be annotated
    /// with CrmEntityAttribute and CrmFieldNameAttribute
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="filterAttributes"></param>
    /// <param name="returnCount"></param>
    /// <returns>The query string</returns>
    public string GetQueryString(string id = null, IList<CrmFilterAttribute> filterAttributes = null, bool returnLinkedEntities = false, bool returnCount = false)
    {
      //Add entity fields
      var query = GetType().GetCustomAttribute<CrmEntityAttribute>().Name + $"{(id == null ? "" : $"({id})")}?$select=" + getOdataNames(GetType().GetProperties());

      if (returnLinkedEntities)
      {
        //Add related entities
        var fkeys = GetType().GetProperties().Where(p => Attribute.GetCustomAttributes(p, typeof(CrmEntityRelationAttribute)).Count() > 0);

        if (fkeys.Count() > 0)
        {
          query += "&$expand=";

          foreach (var fkey in fkeys)
          {
            //Require that the related entity maps to a list
            if (fkey.PropertyType.IsGenericType && fkey.PropertyType.GetGenericTypeDefinition() != typeof(IList<>))
              continue;

            //Get the list item type
            var listMemberType = fkey.PropertyType.GetGenericArguments()[0];

            query += fkey.GetCustomAttribute<CrmEntityRelationAttribute>().Name + "($select=" + getOdataNames(listMemberType.GetProperties()) + "),";
          }
        }

        query = query.TrimEnd(',');
      }

      if (filterAttributes != null)
      {
        if (filterAttributes.Any())
          query += "&$filter=";

        var numberOfResults = filterAttributes.Count;
        var index = 0;
        var openedConditional = false;

        filterAttributes = filterAttributes.OrderByDescending(x => x.MultiConditional == true).ToList();

        for (var i = 0; i < filterAttributes.Count; i++)
        {
          var filterAttribute = filterAttributes[i];

          if (filterAttribute.MultiConditional == true &&
              filterAttributes.Count(x => x.MultiConditional == true) > 1
              && !openedConditional)
          {
            query += "(";
            openedConditional = true;
          }

          query += filterAttribute.FilterName + " eq " + (filterAttribute.QuotesRequired == true ? "'" : "") +
                   filterAttribute.FilterValue + (filterAttribute.QuotesRequired == true ? "'" : "");

          if (++index < numberOfResults)
          {
            if (filterAttributes[i].MultiConditional != true && filterAttributes[i + 1].MultiConditional != true)
              query += " and ";
            else if (filterAttributes[i + 1]?.MultiConditional == true)
            {
              query += " or ";
            }

            if (numberOfResults > 1 && filterAttributes[i].MultiConditional == true &&
                filterAttributes[i + 1].MultiConditional != true)
            {
              query += openedConditional ? ") and " : " and ";
            }
          }

        }
      }

      if (returnCount)
      {
        query += "&$count=true";
      }

      return query;
    }

    public string EntityName => GetType().GetCustomAttribute<CrmEntityAttribute>() != null ? GetType().GetCustomAttribute<CrmEntityAttribute>().Name : string.Empty;

    private string getOdataNames(PropertyInfo[] props)
    {
      var list = "";

      var crmFields = props.SelectMany(p => p.GetCustomAttributes<CrmFieldNameAttribute>());

      if (!crmFields.Any())
        throw new ArgumentException($"No CRM Attributes exist for this type");

      foreach (var field in crmFields)
      {
        list += field.Name + comma;
      }

      return list.TrimEnd(comma);
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
