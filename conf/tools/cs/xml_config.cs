//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: xml serialization ([XmlType]/[XmlElement]) enabled
    
// Generated from: xml_config.proto
namespace Config
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"XmlConfigGroup")]
  [global::System.Xml.Serialization.XmlType(TypeName=@"XmlConfigGroup")]
  public partial class XmlConfigGroup : global::ProtoBuf.IExtensible
  {
    public XmlConfigGroup() {}
    
    // TYPE_MESSAGE.Config.XmlConfigInfo : xml_info
    private  global::System.Collections.Generic.List<Config.XmlConfigInfo> _xml_info = new global::System.Collections.Generic.List<Config.XmlConfigInfo>();
    [global::ProtoBuf.ProtoMember(1, Name=@"xml_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Xml.Serialization.XmlElement(@"xml_info")]
    
    public global::System.Collections.Generic.List<Config.XmlConfigInfo> xml_info
    {
      get { return _xml_info; }
      set { _xml_info = value; }
    }
  
    // TYPE_MESSAGE.Config.SheetConfigInfo : sheet_info
    private  global::System.Collections.Generic.List<Config.SheetConfigInfo> _sheet_info = new global::System.Collections.Generic.List<Config.SheetConfigInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"sheet_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Xml.Serialization.XmlElement(@"sheet_info")]
    
    public global::System.Collections.Generic.List<Config.SheetConfigInfo> sheet_info
    {
      get { return _sheet_info; }
      set { _sheet_info = value; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"XmlConfigInfo")]
  [global::System.Xml.Serialization.XmlType(TypeName=@"XmlConfigInfo")]
  public partial class XmlConfigInfo : global::ProtoBuf.IExtensible
  {
    public XmlConfigInfo() {}
    
    // TYPE_STRING : xml_type
    private string _xml_type = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"xml_type", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Xml.Serialization.XmlAttribute(@"xml_type")]
        
    public string xml_type
    {
      get { return _xml_type; }
      set { _xml_type = value; }
    }
    // TYPE_STRING : path
    private string _path = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"path", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Xml.Serialization.XmlAttribute(@"path")]
        
    public string path
    {
      get { return _path; }
      set { _path = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SheetConfigInfo")]
  [global::System.Xml.Serialization.XmlType(TypeName=@"SheetConfigInfo")]
  public partial class SheetConfigInfo : global::ProtoBuf.IExtensible
  {
    public SheetConfigInfo() {}
    
    // TYPE_STRING : xml_type
    private string _xml_type = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"xml_type", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Xml.Serialization.XmlAttribute(@"xml_type")]
        
    public string xml_type
    {
      get { return _xml_type; }
      set { _xml_type = value; }
    }
    // TYPE_STRING : path
    private string _path = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"path", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Xml.Serialization.XmlAttribute(@"path")]
        
    public string path
    {
      get { return _path; }
      set { _path = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}