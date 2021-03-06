﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by wsdl, Version=4.0.30319.1.
'
Namespace ReversePayment
    
    'CODEGEN: The optional WSDL extension element 'PolicyReference' from namespace 'http://schemas.xmlsoap.org/ws/2004/09/policy' was not handled.
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="ReversePaymentBinding", [Namespace]:="http://xmlns.tigo.com/Service/ReversePayment/V1")>  _
    Partial Public Class ReversePaymentService
        'Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        Inherits Microsoft.Web.Services3.WebServicesClientProtocol
        Private ReversePaymentOperationCompleted As System.Threading.SendOrPostCallback
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = "http://10.11.14.4:7004/osb/services/ReversePayment_1_0"
        End Sub
        
        '''<remarks/>
        Public Event ReversePaymentCompleted As ReversePaymentCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Bare)>  _
        Public Function ReversePayment(<System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://xmlns.tigo.com/ReversePaymentRequest/V1")> ByVal ReversePaymentRequest As ReversePaymentRequest) As <System.Xml.Serialization.XmlElementAttribute("ReversePaymentResponse", [Namespace]:="http://xmlns.tigo.com/ReversePaymentResponse/V1")> ReversePaymentResponse
            Dim results() As Object = Me.Invoke("ReversePayment", New Object() {ReversePaymentRequest})
            Return CType(results(0),ReversePaymentResponse)
        End Function
        
        '''<remarks/>
        Public Function BeginReversePayment(ByVal ReversePaymentRequest As ReversePaymentRequest, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("ReversePayment", New Object() {ReversePaymentRequest}, callback, asyncState)
        End Function
        
        '''<remarks/>
        Public Function EndReversePayment(ByVal asyncResult As System.IAsyncResult) As ReversePaymentResponse
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),ReversePaymentResponse)
        End Function
        
        '''<remarks/>
        Public Overloads Sub ReversePaymentAsync(ByVal ReversePaymentRequest As ReversePaymentRequest)
            Me.ReversePaymentAsync(ReversePaymentRequest, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub ReversePaymentAsync(ByVal ReversePaymentRequest As ReversePaymentRequest, ByVal userState As Object)
            If (Me.ReversePaymentOperationCompleted Is Nothing) Then
                Me.ReversePaymentOperationCompleted = AddressOf Me.OnReversePaymentOperationCompleted
            End If
            Me.InvokeAsync("ReversePayment", New Object() {ReversePaymentRequest}, Me.ReversePaymentOperationCompleted, userState)
        End Sub
        
        Private Sub OnReversePaymentOperationCompleted(ByVal arg As Object)
            If (Not (Me.ReversePaymentCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent ReversePaymentCompleted(Me, New ReversePaymentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentRequest/V1")>  _
    Partial Public Class ReversePaymentRequest
        
        Private requestHeaderField As RequestHeader
        
        Private requestBodyField As ReversePaymentRequestRequestBody
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://xmlns.tigo.com/RequestHeader/V3")>  _
        Public Property RequestHeader() As RequestHeader
            Get
                Return Me.requestHeaderField
            End Get
            Set
                Me.requestHeaderField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property requestBody() As ReversePaymentRequestRequestBody
            Get
                Return Me.requestBodyField
            End Get
            Set
                Me.requestBodyField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/RequestHeader/V3")>  _
    Partial Public Class RequestHeader
        
        Private generalConsumerInformationField As GeneralConsumerInfoType
        
        '''<remarks/>
        Public Property GeneralConsumerInformation() As GeneralConsumerInfoType
            Get
                Return Me.generalConsumerInformationField
            End Get
            Set
                Me.generalConsumerInformationField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xmlns.tigo.com/RequestHeader/V3")>  _
    Partial Public Class GeneralConsumerInfoType
        
        Private consumerIDField As String
        
        Private transactionIDField As String
        
        Private countryField As CountryContentType
        
        Private correlationIDField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property consumerID() As String
            Get
                Return Me.consumerIDField
            End Get
            Set
                Me.consumerIDField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property transactionID() As String
            Get
                Return Me.transactionIDField
            End Get
            Set
                Me.transactionIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property country() As CountryContentType
            Get
                Return Me.countryField
            End Get
            Set
                Me.countryField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property correlationID() As String
            Get
                Return Me.correlationIDField
            End Get
            Set
                Me.correlationIDField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xmlns.tigo.com/RequestHeader/V3")>  _
    Public Enum CountryContentType
        
        '''<remarks/>
        SLV
        
        '''<remarks/>
        GTM
        
        '''<remarks/>
        HND
        
        '''<remarks/>
        BOL
        
        '''<remarks/>
        COL
        
        '''<remarks/>
        PRY
        
        '''<remarks/>
        TCD
        
        '''<remarks/>
        COD
        
        '''<remarks/>
        GHA
        
        '''<remarks/>
        MUS
        
        '''<remarks/>
        RWA
        
        '''<remarks/>
        SEN
        
        '''<remarks/>
        TZA
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xmlns.tigo.com/ResponseHeader/V3")>  _
    Partial Public Class GeneralResponseType
        
        Private correlationIDField As String
        
        Private statusField As System.Nullable(Of StatusContentType)
        
        Private statusFieldSpecified As Boolean
        
        Private codeField As String
        
        Private codeTypeField As String
        
        Private descriptionField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property correlationID() As String
            Get
                Return Me.correlationIDField
            End Get
            Set
                Me.correlationIDField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property status() As System.Nullable(Of StatusContentType)
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property statusSpecified() As Boolean
            Get
                Return Me.statusFieldSpecified
            End Get
            Set
                Me.statusFieldSpecified = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property codeType() As String
            Get
                Return Me.codeTypeField
            End Get
            Set
                Me.codeTypeField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(IsNullable:=true)>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xmlns.tigo.com/ResponseHeader/V3")>  _
    Public Enum StatusContentType
        
        '''<remarks/>
        [ERROR]
        
        '''<remarks/>
        OK
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentRequest/V1")>  _
    Partial Public Class ReversePaymentRequestRequestBody
        
        Private msisdnField As String
        
        Private logIDField As String
        
        Private remarkField As String
        
        Private additionalParametersField As ReversePaymentRequestRequestBodyAdditionalParameters
        
        '''<remarks/>
        Public Property msisdn() As String
            Get
                Return Me.msisdnField
            End Get
            Set
                Me.msisdnField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property logID() As String
            Get
                Return Me.logIDField
            End Get
            Set
                Me.logIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property additionalParameters() As ReversePaymentRequestRequestBodyAdditionalParameters
            Get
                Return Me.additionalParametersField
            End Get
            Set
                Me.additionalParametersField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentRequest/V1")>  _
    Partial Public Class ReversePaymentRequestRequestBodyAdditionalParameters
        
        Private parameterTypeField() As ParameterType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ParameterType", [Namespace]:="http://xmlns.tigo.com/ParameterType/V2")>  _
        Public Property ParameterType() As ParameterType()
            Get
                Return Me.parameterTypeField
            End Get
            Set
                Me.parameterTypeField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ParameterType/V2")>  _
    Partial Public Class ParameterType
        
        Private parameterNameField As String
        
        Private parameterValueField As String
        
        '''<remarks/>
        Public Property parameterName() As String
            Get
                Return Me.parameterNameField
            End Get
            Set
                Me.parameterNameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property parameterValue() As String
            Get
                Return Me.parameterValueField
            End Get
            Set
                Me.parameterValueField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentResponse/V1")>  _
    Partial Public Class ReversePaymentResponse
        
        Private responseHeaderField As ResponseHeader
        
        Private responseBodyField As ReversePaymentResponseResponseBody
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://xmlns.tigo.com/ResponseHeader/V3")>  _
        Public Property ResponseHeader() As ResponseHeader
            Get
                Return Me.responseHeaderField
            End Get
            Set
                Me.responseHeaderField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property responseBody() As ReversePaymentResponseResponseBody
            Get
                Return Me.responseBodyField
            End Get
            Set
                Me.responseBodyField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ResponseHeader/V3")>  _
    Partial Public Class ResponseHeader
        
        Private generalResponseField As GeneralResponseType
        
        '''<remarks/>
        Public Property GeneralResponse() As GeneralResponseType
            Get
                Return Me.generalResponseField
            End Get
            Set
                Me.generalResponseField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentResponse/V1")>  _
    Partial Public Class ReversePaymentResponseResponseBody
        
        Private logIDField As String
        
        Private additionalResultField As ReversePaymentResponseResponseBodyAdditionalResult
        
        '''<remarks/>
        Public Property logID() As String
            Get
                Return Me.logIDField
            End Get
            Set
                Me.logIDField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property additionalResult() As ReversePaymentResponseResponseBodyAdditionalResult
            Get
                Return Me.additionalResultField
            End Get
            Set
                Me.additionalResultField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xmlns.tigo.com/ReversePaymentResponse/V1")>  _
    Partial Public Class ReversePaymentResponseResponseBodyAdditionalResult
        
        Private parameterTypeField() As ParameterType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ParameterType", [Namespace]:="http://xmlns.tigo.com/ParameterType/V2")>  _
        Public Property ParameterType() As ParameterType()
            Get
                Return Me.parameterTypeField
            End Get
            Set
                Me.parameterTypeField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")>  _
    Public Delegate Sub ReversePaymentCompletedEventHandler(ByVal sender As Object, ByVal e As ReversePaymentCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReversePaymentCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As ReversePaymentResponse
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),ReversePaymentResponse)
            End Get
        End Property
    End Class
End Namespace
