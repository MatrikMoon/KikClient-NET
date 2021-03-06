// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: smiley_record.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from smiley_record.proto</summary>
public static partial class SmileyRecordReflection {

  #region Descriptor
  /// <summary>File descriptor for smiley_record.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static SmileyRecordReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChNzbWlsZXlfcmVjb3JkLnByb3RvIl8KDFNtaWxleVJlY29yZBIQCghjYXRl",
          "Z29yeRgBIAEoCRIKCgJpZBgCIAEoCRINCgV0aXRsZRgDIAEoCRIMCgR0ZXh0",
          "GAQgASgJEhQKDGluc3RhbGxfZGF0ZRgFIAEoA2IGcHJvdG8z"));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::SmileyRecord), global::SmileyRecord.Parser, new[]{ "Category", "Id", "Title", "Text", "InstallDate" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class SmileyRecord : pb::IMessage<SmileyRecord> {
  private static readonly pb::MessageParser<SmileyRecord> _parser = new pb::MessageParser<SmileyRecord>(() => new SmileyRecord());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<SmileyRecord> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::SmileyRecordReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public SmileyRecord() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public SmileyRecord(SmileyRecord other) : this() {
    category_ = other.category_;
    id_ = other.id_;
    title_ = other.title_;
    text_ = other.text_;
    installDate_ = other.installDate_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public SmileyRecord Clone() {
    return new SmileyRecord(this);
  }

  /// <summary>Field number for the "category" field.</summary>
  public const int CategoryFieldNumber = 1;
  private string category_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Category {
    get { return category_; }
    set {
      category_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "id" field.</summary>
  public const int IdFieldNumber = 2;
  private string id_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Id {
    get { return id_; }
    set {
      id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "title" field.</summary>
  public const int TitleFieldNumber = 3;
  private string title_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Title {
    get { return title_; }
    set {
      title_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "text" field.</summary>
  public const int TextFieldNumber = 4;
  private string text_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Text {
    get { return text_; }
    set {
      text_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "install_date" field.</summary>
  public const int InstallDateFieldNumber = 5;
  private long installDate_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public long InstallDate {
    get { return installDate_; }
    set {
      installDate_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as SmileyRecord);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(SmileyRecord other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Category != other.Category) return false;
    if (Id != other.Id) return false;
    if (Title != other.Title) return false;
    if (Text != other.Text) return false;
    if (InstallDate != other.InstallDate) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Category.Length != 0) hash ^= Category.GetHashCode();
    if (Id.Length != 0) hash ^= Id.GetHashCode();
    if (Title.Length != 0) hash ^= Title.GetHashCode();
    if (Text.Length != 0) hash ^= Text.GetHashCode();
    if (InstallDate != 0L) hash ^= InstallDate.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (Category.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(Category);
    }
    if (Id.Length != 0) {
      output.WriteRawTag(18);
      output.WriteString(Id);
    }
    if (Title.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(Title);
    }
    if (Text.Length != 0) {
      output.WriteRawTag(34);
      output.WriteString(Text);
    }
    if (InstallDate != 0L) {
      output.WriteRawTag(40);
      output.WriteInt64(InstallDate);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Category.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Category);
    }
    if (Id.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
    }
    if (Title.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Title);
    }
    if (Text.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Text);
    }
    if (InstallDate != 0L) {
      size += 1 + pb::CodedOutputStream.ComputeInt64Size(InstallDate);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(SmileyRecord other) {
    if (other == null) {
      return;
    }
    if (other.Category.Length != 0) {
      Category = other.Category;
    }
    if (other.Id.Length != 0) {
      Id = other.Id;
    }
    if (other.Title.Length != 0) {
      Title = other.Title;
    }
    if (other.Text.Length != 0) {
      Text = other.Text;
    }
    if (other.InstallDate != 0L) {
      InstallDate = other.InstallDate;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          Category = input.ReadString();
          break;
        }
        case 18: {
          Id = input.ReadString();
          break;
        }
        case 26: {
          Title = input.ReadString();
          break;
        }
        case 34: {
          Text = input.ReadString();
          break;
        }
        case 40: {
          InstallDate = input.ReadInt64();
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code
