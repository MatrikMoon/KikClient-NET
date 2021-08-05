// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: messagepath/v1/core_message_options.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Common.Messagepath.V1 {

  /// <summary>Holder for reflection information generated from messagepath/v1/core_message_options.proto</summary>
  public static partial class CoreMessageOptionsReflection {

    #region Descriptor
    /// <summary>File descriptor for messagepath/v1/core_message_options.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CoreMessageOptionsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiltZXNzYWdlcGF0aC92MS9jb3JlX21lc3NhZ2Vfb3B0aW9ucy5wcm90bxIV",
            "Y29tbW9uLm1lc3NhZ2VwYXRoLnYxGiBnb29nbGUvcHJvdG9idWYvZGVzY3Jp",
            "cHRvci5wcm90byKHAQocQ29yZU1lc3NhZ2VPcmlnaW5SZXN0cmljdGlvbhJI",
            "CgRkZW55GAEgAygOMjouY29tbW9uLm1lc3NhZ2VwYXRoLnYxLkNvcmVNZXNz",
            "YWdlT3JpZ2luUmVzdHJpY3Rpb24uT3JpZ2luIh0KBk9yaWdpbhIKCgZNT0JJ",
            "TEUQABIHCgNCT1QQATpwChJvcmlnaW5fcmVzdHJpY3Rpb24SHS5nb29nbGUu",
            "cHJvdG9idWYuRmllbGRPcHRpb25zGNvTBCABKAsyMy5jb21tb24ubWVzc2Fn",
            "ZXBhdGgudjEuQ29yZU1lc3NhZ2VPcmlnaW5SZXN0cmljdGlvbkJwChljb20u",
            "a2lrLm1lc3NhZ2VwYXRoLm1vZGVsWlNnaXRodWIuY29tL2tpa2ludGVyYWN0",
            "aXZlL3hpcGhpYXMtbW9kZWwtY29tbW9uL2dlbmVyYXRlZC9nby9tZXNzYWdl",
            "cGF0aDttZXNzYWdlcGF0aA=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.Reflection.DescriptorReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pb::Extension[] { CoreMessageOptionsExtensions.OriginRestriction }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Common.Messagepath.V1.CoreMessageOriginRestriction), global::Common.Messagepath.V1.CoreMessageOriginRestriction.Parser, new[]{ "Deny" }, null, new[]{ typeof(global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin) }, null, null)
          }));
    }
    #endregion

  }
  /// <summary>Holder for extension identifiers generated from the top level of messagepath/v1/core_message_options.proto</summary>
  public static partial class CoreMessageOptionsExtensions {
    /// <summary>
    /// This option is used to document and control what CoreMessage attachments (fields) can be set
    /// based on where the message originates from. Currently (Oct 2016), this option should only be
    /// used on top level CoreMessage fields (ie: checking is not done recursively)
    ///
    /// NOTE: protobuf_validation.proto contains the master list of extension numbers used at kik
    /// </summary>
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.FieldOptions, global::Common.Messagepath.V1.CoreMessageOriginRestriction> OriginRestriction =
      new pb::Extension<global::Google.Protobuf.Reflection.FieldOptions, global::Common.Messagepath.V1.CoreMessageOriginRestriction>(76251, pb::FieldCodec.ForMessage(610010, global::Common.Messagepath.V1.CoreMessageOriginRestriction.Parser));
  }

  #region Messages
  public sealed partial class CoreMessageOriginRestriction : pb::IMessage<CoreMessageOriginRestriction>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CoreMessageOriginRestriction> _parser = new pb::MessageParser<CoreMessageOriginRestriction>(() => new CoreMessageOriginRestriction());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CoreMessageOriginRestriction> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Common.Messagepath.V1.CoreMessageOptionsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CoreMessageOriginRestriction() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CoreMessageOriginRestriction(CoreMessageOriginRestriction other) : this() {
      deny_ = other.deny_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CoreMessageOriginRestriction Clone() {
      return new CoreMessageOriginRestriction(this);
    }

    /// <summary>Field number for the "deny" field.</summary>
    public const int DenyFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin> _repeated_deny_codec
        = pb::FieldCodec.ForEnum(8, x => (int) x, x => (global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin) x);
    private readonly pbc::RepeatedField<global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin> deny_ = new pbc::RepeatedField<global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin>();
    /// <summary>
    /// The list of origins which are not allowed to use the particular attachment when sending
    /// messages
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Common.Messagepath.V1.CoreMessageOriginRestriction.Types.Origin> Deny {
      get { return deny_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CoreMessageOriginRestriction);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CoreMessageOriginRestriction other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!deny_.Equals(other.deny_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= deny_.GetHashCode();
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
      deny_.WriteTo(output, _repeated_deny_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      deny_.WriteTo(ref output, _repeated_deny_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += deny_.CalculateSize(_repeated_deny_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CoreMessageOriginRestriction other) {
      if (other == null) {
        return;
      }
      deny_.Add(other.deny_);
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
          case 10:
          case 8: {
            deny_.AddEntriesFrom(input, _repeated_deny_codec);
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
          case 10:
          case 8: {
            deny_.AddEntriesFrom(ref input, _repeated_deny_codec);
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the CoreMessageOriginRestriction message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum Origin {
        /// <summary>
        /// Native mobile clients connecting over XMPP
        /// </summary>
        [pbr::OriginalName("MOBILE")] Mobile = 0,
        /// <summary>
        /// Bot platform bots sending messages over the WebMessagingBridge (As of Oct 2016)
        /// </summary>
        [pbr::OriginalName("BOT")] Bot = 1,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code