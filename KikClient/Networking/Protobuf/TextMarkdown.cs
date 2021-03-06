// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: messagepath/v1/text_markdown.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Common.Messagepath.V1 {

  /// <summary>Holder for reflection information generated from messagepath/v1/text_markdown.proto</summary>
  public static partial class TextMarkdownReflection {

    #region Descriptor
    /// <summary>File descriptor for messagepath/v1/text_markdown.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TextMarkdownReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiJtZXNzYWdlcGF0aC92MS90ZXh0X21hcmtkb3duLnByb3RvEhVjb21tb24u",
            "bWVzc2FnZXBhdGgudjEaGXByb3RvYnVmX3ZhbGlkYXRpb24ucHJvdG8iMwoW",
            "VGV4dE1hcmtkb3duQXR0YWNobWVudBIZCghtYXJrZG93bhgBIAEoCUIHyp0l",
            "AzDEE0J3Chljb20ua2lrLm1lc3NhZ2VwYXRoLm1vZGVsWlNnaXRodWIuY29t",
            "L2tpa2ludGVyYWN0aXZlL3hpcGhpYXMtbW9kZWwtY29tbW9uL2dlbmVyYXRl",
            "ZC9nby9tZXNzYWdlcGF0aDttZXNzYWdlcGF0aKICBE1QVEhiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Kik.Validation.ProtobufValidationReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Common.Messagepath.V1.TextMarkdownAttachment), global::Common.Messagepath.V1.TextMarkdownAttachment.Parser, new[]{ "Markdown" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// TextMarkdownAttachment defines the mechanism that allows bots to send a text message containing
  /// text with markdown.
  ///
  /// This attachment should be used by the client to render text with markdown.
  /// </summary>
  public sealed partial class TextMarkdownAttachment : pb::IMessage<TextMarkdownAttachment>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<TextMarkdownAttachment> _parser = new pb::MessageParser<TextMarkdownAttachment>(() => new TextMarkdownAttachment());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TextMarkdownAttachment> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Common.Messagepath.V1.TextMarkdownReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TextMarkdownAttachment() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TextMarkdownAttachment(TextMarkdownAttachment other) : this() {
      markdown_ = other.markdown_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TextMarkdownAttachment Clone() {
      return new TextMarkdownAttachment(this);
    }

    /// <summary>Field number for the "markdown" field.</summary>
    public const int MarkdownFieldNumber = 1;
    private string markdown_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Markdown {
      get { return markdown_; }
      set {
        markdown_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TextMarkdownAttachment);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TextMarkdownAttachment other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Markdown != other.Markdown) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Markdown.Length != 0) hash ^= Markdown.GetHashCode();
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
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Markdown.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Markdown);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Markdown.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Markdown);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Markdown.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Markdown);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TextMarkdownAttachment other) {
      if (other == null) {
        return;
      }
      if (other.Markdown.Length != 0) {
        Markdown = other.Markdown;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Markdown = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Markdown = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
