// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: safetynet_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mobile.Antispam.Safetynet.V1 {

  /// <summary>Holder for reflection information generated from safetynet_service.proto</summary>
  public static partial class SafetynetServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for safetynet_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SafetynetServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdzYWZldHluZXRfc2VydmljZS5wcm90bxIcbW9iaWxlLmFudGlzcGFtLnNh",
            "ZmV0eW5ldC52MRoQY29tbW9uX3JwYy5wcm90bxoZcHJvdG9idWZfdmFsaWRh",
            "dGlvbi5wcm90byKEAQoQR2V0Tm9uY2VSZXNwb25zZRJFCgZyZXN1bHQYASAB",
            "KA4yNS5tb2JpbGUuYW50aXNwYW0uc2FmZXR5bmV0LnYxLkdldE5vbmNlUmVz",
            "cG9uc2UuUmVzdWx0EhcKBW5vbmNlGAIgASgJQgjKnSUECAEoECIQCgZSZXN1",
            "bHQSBgoCT0sQACI7Ch5WZXJpZnlBdHRlc3RhdGlvblJlc3VsdFJlcXVlc3QS",
            "GQoDandzGAEgASgJQgzKnSUICAEoATCowwEiswEKH1ZlcmlmeUF0dGVzdGF0",
            "aW9uUmVzdWx0UmVzcG9uc2USVAoGcmVzdWx0GAEgASgOMkQubW9iaWxlLmFu",
            "dGlzcGFtLnNhZmV0eW5ldC52MS5WZXJpZnlBdHRlc3RhdGlvblJlc3VsdFJl",
            "c3BvbnNlLlJlc3VsdCI6CgZSZXN1bHQSBgoCT0sQABIRCg1JTlZBTElEX05P",
            "TkNFEAESFQoRTUFMRk9STUVEX1JFUVVFU1QQAjL5AQoJU2FmZXR5TmV0ElEK",
            "CEdldE5vbmNlEhMuY29tbW9uLlZvaWRSZXF1ZXN0Gi4ubW9iaWxlLmFudGlz",
            "cGFtLnNhZmV0eW5ldC52MS5HZXROb25jZVJlc3BvbnNlIgASmAEKF1Zlcmlm",
            "eUF0dGVzdGF0aW9uUmVzdWx0EjwubW9iaWxlLmFudGlzcGFtLnNhZmV0eW5l",
            "dC52MS5WZXJpZnlBdHRlc3RhdGlvblJlc3VsdFJlcXVlc3QaPS5tb2JpbGUu",
            "YW50aXNwYW0uc2FmZXR5bmV0LnYxLlZlcmlmeUF0dGVzdGF0aW9uUmVzdWx0",
            "UmVzcG9uc2UiAEJ7Ch5jb20ua2lrLmFudGlzcGFtLnNhZmV0eW5ldC5ycGNa",
            "WWdpdGh1Yi5jb20va2lraW50ZXJhY3RpdmUveGlwaGlhcy1hcGktbW9iaWxl",
            "L2dlbmVyYXRlZC9nby9hbnRpc3BhbS9zYWZldHluZXQvdjE7c2FmZXR5bmV0",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Common.CommonRpcReflection.Descriptor, global::Kik.Validation.ProtobufValidationReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mobile.Antispam.Safetynet.V1.GetNonceResponse), global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Parser, new[]{ "Result", "Nonce" }, null, new[]{ typeof(global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result) }, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultRequest), global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultRequest.Parser, new[]{ "Jws" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse), global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Parser, new[]{ "Result" }, null, new[]{ typeof(global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result) }, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class GetNonceResponse : pb::IMessage<GetNonceResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetNonceResponse> _parser = new pb::MessageParser<GetNonceResponse>(() => new GetNonceResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetNonceResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mobile.Antispam.Safetynet.V1.SafetynetServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetNonceResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetNonceResponse(GetNonceResponse other) : this() {
      result_ = other.result_;
      nonce_ = other.nonce_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetNonceResponse Clone() {
      return new GetNonceResponse(this);
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 1;
    private global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result result_ = global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    /// <summary>Field number for the "nonce" field.</summary>
    public const int NonceFieldNumber = 2;
    private string nonce_ = "";
    /// <summary>
    /// A nonce that must be signed via the attestation API in order to validate with the Kik SafetyNet verification
    /// service backend.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Nonce {
      get { return nonce_; }
      set {
        nonce_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetNonceResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetNonceResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Result != other.Result) return false;
      if (Nonce != other.Nonce) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Result != global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok) hash ^= Result.GetHashCode();
      if (Nonce.Length != 0) hash ^= Nonce.GetHashCode();
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
      if (Result != global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Result);
      }
      if (Nonce.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Nonce);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Result != global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Result);
      }
      if (Nonce.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Nonce);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Result != global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Result);
      }
      if (Nonce.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Nonce);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetNonceResponse other) {
      if (other == null) {
        return;
      }
      if (other.Result != global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result.Ok) {
        Result = other.Result;
      }
      if (other.Nonce.Length != 0) {
        Nonce = other.Nonce;
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
          case 8: {
            Result = (global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result) input.ReadEnum();
            break;
          }
          case 18: {
            Nonce = input.ReadString();
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
          case 8: {
            Result = (global::Mobile.Antispam.Safetynet.V1.GetNonceResponse.Types.Result) input.ReadEnum();
            break;
          }
          case 18: {
            Nonce = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the GetNonceResponse message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum Result {
        [pbr::OriginalName("OK")] Ok = 0,
      }

    }
    #endregion

  }

  public sealed partial class VerifyAttestationResultRequest : pb::IMessage<VerifyAttestationResultRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<VerifyAttestationResultRequest> _parser = new pb::MessageParser<VerifyAttestationResultRequest>(() => new VerifyAttestationResultRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VerifyAttestationResultRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mobile.Antispam.Safetynet.V1.SafetynetServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultRequest(VerifyAttestationResultRequest other) : this() {
      jws_ = other.jws_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultRequest Clone() {
      return new VerifyAttestationResultRequest(this);
    }

    /// <summary>Field number for the "jws" field.</summary>
    public const int JwsFieldNumber = 1;
    private string jws_ = "";
    /// <summary>
    /// The full JWS result from the Google SafetyNet attestation API.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Jws {
      get { return jws_; }
      set {
        jws_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VerifyAttestationResultRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VerifyAttestationResultRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Jws != other.Jws) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Jws.Length != 0) hash ^= Jws.GetHashCode();
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
      if (Jws.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Jws);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Jws.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Jws);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Jws.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Jws);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VerifyAttestationResultRequest other) {
      if (other == null) {
        return;
      }
      if (other.Jws.Length != 0) {
        Jws = other.Jws;
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
            Jws = input.ReadString();
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
            Jws = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class VerifyAttestationResultResponse : pb::IMessage<VerifyAttestationResultResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<VerifyAttestationResultResponse> _parser = new pb::MessageParser<VerifyAttestationResultResponse>(() => new VerifyAttestationResultResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VerifyAttestationResultResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mobile.Antispam.Safetynet.V1.SafetynetServiceReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultResponse(VerifyAttestationResultResponse other) : this() {
      result_ = other.result_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VerifyAttestationResultResponse Clone() {
      return new VerifyAttestationResultResponse(this);
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 1;
    private global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result result_ = global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VerifyAttestationResultResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VerifyAttestationResultResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Result != other.Result) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Result != global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok) hash ^= Result.GetHashCode();
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
      if (Result != global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Result);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Result != global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Result);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Result != global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Result);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VerifyAttestationResultResponse other) {
      if (other == null) {
        return;
      }
      if (other.Result != global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result.Ok) {
        Result = other.Result;
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
          case 8: {
            Result = (global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result) input.ReadEnum();
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
          case 8: {
            Result = (global::Mobile.Antispam.Safetynet.V1.VerifyAttestationResultResponse.Types.Result) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the VerifyAttestationResultResponse message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum Result {
        /// <summary>
        /// The request was syntactically well-formed with a valid nonce. The client need not retry.
        /// </summary>
        [pbr::OriginalName("OK")] Ok = 0,
        /// <summary>
        /// The nonce provided either expired, or did not match the last nonce provided to the client.
        /// </summary>
        [pbr::OriginalName("INVALID_NONCE")] InvalidNonce = 1,
        /// <summary>
        /// The request was syntactically malformed (invalid JWS).
        /// </summary>
        [pbr::OriginalName("MALFORMED_REQUEST")] MalformedRequest = 2,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
