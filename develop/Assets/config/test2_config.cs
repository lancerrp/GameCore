//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: xml serialization ([XmlType]/[XmlElement]) enabled
    
// Generated from: xml_proto/test2_config.proto
namespace Config
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Test2Config")]
  [global::System.Xml.Serialization.XmlType(TypeName=@"Test2Config")]
  public partial class Test2Config : global::ProtoBuf.IExtensible
  {
    public Test2Config() {}
    
    // TYPE_INT32 : id
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    [global::System.Xml.Serialization.XmlAttribute(@"id")]
        
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}