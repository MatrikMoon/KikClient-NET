// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: messagepath/v1/visibility_rules.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Common.Messagepath.V1 {

  /// <summary>Holder for reflection information generated from messagepath/v1/visibility_rules.proto</summary>
  public static partial class VisibilityRulesReflection {

    #region Descriptor
    /// <summary>File descriptor for messagepath/v1/visibility_rules.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static VisibilityRulesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiVtZXNzYWdlcGF0aC92MS92aXNpYmlsaXR5X3J1bGVzLnByb3RvEhVjb21t",
            "b24ubWVzc2FnZXBhdGgudjEaEmNvbW1vbl9tb2RlbC5wcm90bxoZcHJvdG9i",
            "dWZfdmFsaWRhdGlvbi5wcm90byL5AQoZVmlzaWJpbGl0eVJ1bGVzQXR0YWNo",
            "bWVudBIoCglpbml0aWF0b3IYASABKAsyFS5jb21tb24uWGlCYXJlVXNlckpp",
            "ZBIkChxkcm9wX2lmX2luaXRpYXRvcl9ub3RfZnJpZW5kGAIgASgIEkMKBHJ1",
            "bGUYAyABKA4yNS5jb21tb24ubWVzc2FnZXBhdGgudjEuVmlzaWJpbGl0eVJ1",
            "bGVzQXR0YWNobWVudC5SdWxlIkcKBFJ1bGUSHQoZVVNFX1NFTkRFUl9GT1Jf",
            "VklTSUJJTElUWRAAEiAKHFVTRV9JTklUSUFUT1JfRk9SX1ZJU0lCSUxJVFkQ",
            "AUJ3Chljb20ua2lrLm1lc3NhZ2VwYXRoLm1vZGVsWlNnaXRodWIuY29tL2tp",
            "a2ludGVyYWN0aXZlL3hpcGhpYXMtbW9kZWwtY29tbW9uL2dlbmVyYXRlZC9n",
            "by9tZXNzYWdlcGF0aDttZXNzYWdlcGF0aKICBE1QVEhiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Common.CommonModelReflection.Descriptor, global::Kik.Validation.ProtobufValidationReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Common.Messagepath.V1.VisibilityRulesAttachment), global::Common.Messagepath.V1.VisibilityRulesAttachment.Parser, new[]{ "Initiator", "DropIfInitiatorNotFriend", "Rule" }, null, new[]{ typeof(global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule) }, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// 'Visibility' relates to:
  ///      - Where the convo appears (new people/main chat)
  ///      - How push is sent
  ///      - What content is blurred
  ///      - What profile pics are blurred
  ///      - If bottom bar shows up
  ///
  /// This attachment represents a minimal set of overrides to the default rules the client applies.
  /// </summary>
  public sealed partial class VisibilityRulesAttachment : pb::IMessage<VisibilityRulesAttachment>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<VisibilityRulesAttachment> _parser = new pb::MessageParser<VisibilityRulesAttachment>(() => new VisibilityRulesAttachment());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VisibilityRulesAttachment> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Common.Messagepath.V1.VisibilityRulesReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VisibilityRulesAttachment() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VisibilityRulesAttachment(VisibilityRulesAttachment other) : this() {
      initiator_ = other.initiator_ != null ? other.initiator_.Clone() : null;
      dropIfInitiatorNotFriend_ = other.dropIfInitiatorNotFriend_;
      rule_ = other.rule_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VisibilityRulesAttachment Clone() {
      return new VisibilityRulesAttachment(this);
    }

    /// <summary>Field number for the "initiator" field.</summary>
    public const int InitiatorFieldNumber = 1;
    private global::Common.XiBareUserJid initiator_;
    /// <summary>
    /// The initiator is distinctly different from the sender of the message.
    /// It SHOULD NOT be set to the same value as the sender.
    /// The initiator can be a user or a bot.
    /// This field is OPTIONAL.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Common.XiBareUserJid Initiator {
      get { return initiator_; }
      set {
        initiator_ = value;
      }
    }

    /// <summary>Field number for the "drop_if_initiator_not_friend" field.</summary>
    public const int DropIfInitiatorNotFriendFieldNumber = 2;
    private bool dropIfInitiatorNotFriend_;
    /// <summary>
    /// 'Friend' is defined as:   (in roster AND not blocked) OR yourself
    /// Push should NEVER be sent if the initiator is not a friend.
    /// The server WILL NOT filter these messages, it is up to the receiving client to drop, but still ack (if necessary), the message.
    /// If the initiator is a friend, continue processing the other rules defined in this attachment.
    /// The initiator field SHOULD be set, if this field is true. If initiator is not set, this option is ignored.
    /// Initial usecase for this is viral invites for bots (https://docs.google.com/document/d/1v4JtP1fdah5cvgXW2apScf_bemMkrvh_J370X5jJD48).
    /// Message should be dropped by the client, not simply hidden (although acked through QoS as necessary).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool DropIfInitiatorNotFriend {
      get { return dropIfInitiatorNotFriend_; }
      set {
        dropIfInitiatorNotFriend_ = value;
      }
    }

    /// <summary>Field number for the "rule" field.</summary>
    public const int RuleFieldNumber = 3;
    private global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule rule_ = global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility;
    /// <summary>
    /// If UKNOWN, use the default rule: USE_SENDER_FOR_VISIBILITY (ie: for forwards compatibility).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule Rule {
      get { return rule_; }
      set {
        rule_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VisibilityRulesAttachment);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VisibilityRulesAttachment other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Initiator, other.Initiator)) return false;
      if (DropIfInitiatorNotFriend != other.DropIfInitiatorNotFriend) return false;
      if (Rule != other.Rule) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (initiator_ != null) hash ^= Initiator.GetHashCode();
      if (DropIfInitiatorNotFriend != false) hash ^= DropIfInitiatorNotFriend.GetHashCode();
      if (Rule != global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility) hash ^= Rule.GetHashCode();
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
      if (initiator_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Initiator);
      }
      if (DropIfInitiatorNotFriend != false) {
        output.WriteRawTag(16);
        output.WriteBool(DropIfInitiatorNotFriend);
      }
      if (Rule != global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Rule);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (initiator_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Initiator);
      }
      if (DropIfInitiatorNotFriend != false) {
        output.WriteRawTag(16);
        output.WriteBool(DropIfInitiatorNotFriend);
      }
      if (Rule != global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Rule);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (initiator_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Initiator);
      }
      if (DropIfInitiatorNotFriend != false) {
        size += 1 + 1;
      }
      if (Rule != global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Rule);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VisibilityRulesAttachment other) {
      if (other == null) {
        return;
      }
      if (other.initiator_ != null) {
        if (initiator_ == null) {
          Initiator = new global::Common.XiBareUserJid();
        }
        Initiator.MergeFrom(other.Initiator);
      }
      if (other.DropIfInitiatorNotFriend != false) {
        DropIfInitiatorNotFriend = other.DropIfInitiatorNotFriend;
      }
      if (other.Rule != global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule.UseSenderForVisibility) {
        Rule = other.Rule;
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
            if (initiator_ == null) {
              Initiator = new global::Common.XiBareUserJid();
            }
            input.ReadMessage(Initiator);
            break;
          }
          case 16: {
            DropIfInitiatorNotFriend = input.ReadBool();
            break;
          }
          case 24: {
            Rule = (global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule) input.ReadEnum();
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
            if (initiator_ == null) {
              Initiator = new global::Common.XiBareUserJid();
            }
            input.ReadMessage(Initiator);
            break;
          }
          case 16: {
            DropIfInitiatorNotFriend = input.ReadBool();
            break;
          }
          case 24: {
            Rule = (global::Common.Messagepath.V1.VisibilityRulesAttachment.Types.Rule) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the VisibilityRulesAttachment message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum Rule {
        /// <summary>
        /// Use this rule if the current value is UNKNOWN (for forwards compatibility).
        /// Similar to the default client behaviour, use the sender for determining push and convo location.
        /// Respect the blocked status of the sender AND initiator: if either is blocked, DO NOT send push.
        /// Initial usecase for this is viral invites  for bots (https://docs.google.com/document/d/1v4JtP1fdah5cvgXW2apScf_bemMkrvh_J370X5jJD48).
        /// Note that for the viral invites case, the sender should always be a bot.
        /// </summary>
        [pbr::OriginalName("USE_SENDER_FOR_VISIBILITY")] UseSenderForVisibility = 0,
        /// <summary>
        /// Convo location (new pople/main list) and push behavior should be based on the initiator (if present).
        /// Respect the blocked status of the sender AND initiator: if either is blocked, DO NOT send push.
        /// The initiator field SHOULD be set but if the initiator field is not set, use the sender for all visibility rules.
        /// Initial usecase for this is mention replies from bots to users (https://kikinteractive.atlassian.net/browse/SERVER-257)
        /// </summary>
        [pbr::OriginalName("USE_INITIATOR_FOR_VISIBILITY")] UseInitiatorForVisibility = 1,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
