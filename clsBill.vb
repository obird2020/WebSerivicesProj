Imports getinvoicedetail = GhEpinDLL.GetInvoiceDetail
Imports getSecurityDetail = GhEpinDLL.GetSecurityDepositDetails
Imports ManageSecurity = GhEpinDLL.ManageSecurityDeposit
Imports RegisterPayment = GhEpinDLL.RegisterPayment
Imports ReverPayment = GhEpinDLL.ReversePayment
Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Xml
Imports System.Net.Cache
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Configuration

'Imports System.Net


Public Class clsBill

    Implements IDisposable
    Public Function GetInvoiceDetail(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                      ByVal typ As Long, ByVal msisdn As String, ByVal accountcode As String) As String
        On Error GoTo err
        ' 41.6:8011

        Using proxy As New GetInvoiceDetail.GetInvoiceDetailService
            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            ' Use the WSE 3.0 security token class...
            'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim getInvoice As New GetInvoiceDetail.GetInvoiceDetailRequest

            getInvoice.RequestHeader = New GetInvoiceDetail.RequestHeader

            getInvoice.RequestHeader.GeneralConsumerInformation = New GetInvoiceDetail.GeneralConsumerInfoType()
            getInvoice.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            getInvoice.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation()
            getInvoice.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.GetInvoiceDetail.CountryContentType.GHA
            getInvoice.RequestHeader.GeneralConsumerInformation.transactionID = ""

            getInvoice.requestBody = New GetInvoiceDetail.GetInvoiceDetailRequestRequestBody
            getInvoice.requestBody.msisdn = msisdn

            If typ = 0 Then
                getInvoice.requestBody.ItemElementName = GhEpinDLL.GetInvoiceDetail.ItemChoiceType.acctId
            Else
                getInvoice.requestBody.ItemElementName = GhEpinDLL.GetInvoiceDetail.ItemChoiceType.acctCode
            End If
            getInvoice.requestBody.Item = accountcode


            Dim response As GetInvoiceDetail.GetInvoiceDetailResponse = proxy.GetInvoiceDetail(getInvoice)

            Dim invoice() As GhEpinDLL.GetInvoiceDetail.invoiceDetailType

            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.GetInvoiceDetail.StatusContentType.OK Then
                invoice = response.responseBody.invoiceDetailList

                Return invoice(0).acctCode & "|" & invoice(0).acctId & "|" & invoice(0).BillCycleId & "|" & invoice(0).custCode _
                        & "|" & invoice(0).custId & "|" & invoice(0).msisdn & "|" & invoice(0).invoiceId & "|" & invoice(0).invoiceNo _
                        & "|" & invoice(0).InvoiceAmount & "|" & invoice(0).invoiceDate & "|" & invoice(0).OpenAmount & "|" & invoice(0).dueDate _
                        & "|" & invoice(0).SettleDate & "|" & invoice(0).currencyId & "|" & invoice(0).subId


            Else
                Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                    & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description
            End If
            'Catch ex As Exception

            'End Try


        End Using

err:

        Return Err.Description
    End Function

    Public Function GetSecurityDepositDetails(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                              ByVal invoiceid As String, ByVal accountid As String, flag As Integer) As String
        On Error GoTo err
        ' 41.6:8011

        Using proxy As New GetSecurityDepositDetails.GetSecurityDepositDetailsService

            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            ' Use the WSE 3.0 security token class...
            'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim getdeposit As New GetSecurityDepositDetails.GetSecurityDepositDetailsRequest

            getdeposit.RequestHeader = New GetSecurityDepositDetails.RequestHeader

            getdeposit.RequestHeader.GeneralConsumerInformation = New GetSecurityDepositDetails.GeneralConsumerInfoType()
            getdeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            getdeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation() '"4c153812-8a13-4f48-81e2-e68fcbf48e39"
            getdeposit.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.GetSecurityDepositDetails.CountryContentType.GHA
            getdeposit.RequestHeader.GeneralConsumerInformation.transactionID = ""

            getdeposit.requestBody = New GetSecurityDepositDetails.GetSecurityDepositDetailsRequestRequestBody
            getdeposit.requestBody.acctId = accountid
            getdeposit.requestBody.invoiceId = invoiceid
            getdeposit.requestBody.paidFlag = CType(flag, GhEpinDLL.GetSecurityDepositDetails.GetSecurityDepositDetailsRequestRequestBodyPaidFlag)


            Dim response As GetSecurityDepositDetails.GetSecurityDepositDetailsResponse = proxy.GetSecurityDepositDetails(getdeposit)
            Dim deposit() As GhEpinDLL.GetSecurityDepositDetails.GetSecurityDepositDetailsResponseResponseBodyDepositDetailResultValue
            Dim depositList() As GhEpinDLL.GetSecurityDepositDetails.GetSecurityDepositDetailsResponseResponseBodyDepositDetailResultValueDepPaymentValue


            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.GetSecurityDepositDetails.StatusContentType.OK Then
                deposit = response.responseBody.DepositDetailResultList
                depositList = deposit(0).DepPaymentList

                Return deposit(0).AcctId & "|" & deposit(0).Msisdn & "|" & deposit(0).SubId & "|" & deposit(0).DepositType _
                        & "|" & deposit(0).InvoiceAmount & "|" & deposit(0).OpenAmount _
                        & "|" & deposit(0).Status & "|" & deposit(0).EntryDate & "|" & depositList(0).PaymentMethod & "|" & depositList(0).Amount

            Else
                Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                    & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description
            End If


        End Using

err:

        Return Err.Description
    End Function

    Public Function ManageSecurityDeposit(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                          ByVal msisdn As String, ByVal accountnumber As String, ByVal paymethod As Integer, _
                                          ByVal amt As Double, ByVal charge As Boolean) As String
        On Error GoTo err
        ' 41.6:8011

        Using proxy As New ManageSecurity.ManageSecurityDeposit
            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            ' Use the WSE 3.0 security token class...
            'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim manageDeposit As New ManageSecurity.SecurityDeposit

            manageDeposit.RequestHeader = New ManageSecurityDeposit.RequestHeader

            manageDeposit.RequestHeader.GeneralConsumerInformation = New ManageSecurityDeposit.GeneralConsumerInfoType
            manageDeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            manageDeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation() '"4c153812-8a13-4f48-81e2-e68fcbf48e39"
            manageDeposit.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.ManageSecurityDeposit.CountryContentType.GHA
            manageDeposit.RequestHeader.GeneralConsumerInformation.transactionID = ""

            manageDeposit.RequestBody = New ManageSecurityDeposit.SecurityDepositRequestBody
            manageDeposit.RequestBody.msisdn = msisdn
            manageDeposit.RequestBody.accountNumber = accountnumber
            manageDeposit.RequestBody.chargeCustomer = True

            Dim pay As New ManageSecurityDeposit.paymentDetail
            pay.paymentMethod = CType(paymethod, GhEpinDLL.ManageSecurityDeposit.paymentMethod)
            pay.paymentAmount = CDec(amt)
            pay.chequeNumber = ""
            pay.bankCode = ""
            pay.cardNumber = ""
            pay.cardType = ""
            pay.cardBatchNumber = ""
            pay.approvalCode = ""
            pay.remark = ""

            manageDeposit.RequestBody.paymentDetail = pay

            Dim response As ManageSecurityDeposit.SecurityDepositResponse = proxy.SecurityDeposit(manageDeposit)

            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then
                Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description _
                        & "|" & response.ResponseBody.transactionReference & "|" & response.ResponseBody.depositAmount

            Else
                Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                    & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description
            End If

        End Using

err:

        Return Err.Description
    End Function

    Public Function RefundSecurityDeposit(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                          ByVal msisdn As String, ByVal iscustomer As Integer, typ As Integer, ByVal accountcodeid As String, ByVal reftyp As Integer, _
                                          ByVal amt As Double) As String
        On Error GoTo err
        ' 41.6:8011

        Using proxy As New ManageSecurity.ManageSecurityDeposit
            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            ' Use the WSE 3.0 security token class...
            'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim refundDeposit As New ManageSecurity.RefundSecurityDeposit

            refundDeposit.RequestHeader = New ManageSecurityDeposit.RequestHeader

            refundDeposit.RequestHeader.GeneralConsumerInformation = New ManageSecurityDeposit.GeneralConsumerInfoType
            refundDeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            refundDeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation() '"4c153812-8a13-4f48-81e2-e68fcbf48e39"
            refundDeposit.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.ManageSecurityDeposit.CountryContentType.GHA
            refundDeposit.RequestHeader.GeneralConsumerInformation.transactionID = ""

            refundDeposit.RequestBody = New ManageSecurityDeposit.RefundSecurityDepositRequestBody

            If iscustomer = 0 Then
                refundDeposit.RequestBody.isCustomer = GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyIsCustomer.No
            Else
                refundDeposit.RequestBody.isCustomer = GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyIsCustomer.Yes
            End If

            If typ = 0 Then
                refundDeposit.RequestBody.ItemElementName = GhEpinDLL.ManageSecurityDeposit.ItemChoiceType.acctId
            Else
                refundDeposit.RequestBody.ItemElementName = GhEpinDLL.ManageSecurityDeposit.ItemChoiceType.acctCode
            End If



            refundDeposit.RequestBody.Item = CStr(accountcodeid)
            refundDeposit.RequestBody.msisdn = msisdn
            refundDeposit.RequestBody.subId = ""
            refundDeposit.RequestBody.custName = ""
            refundDeposit.RequestBody.refundType = CType(reftyp, GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyRefundType)
            refundDeposit.RequestBody.amount = CLng(amt)
            refundDeposit.RequestBody.bankAcctType = GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyBankAcctType.Other
            refundDeposit.RequestBody.bankBranchNumber = ""
            refundDeposit.RequestBody.bankAcctNo = ""
            refundDeposit.RequestBody.contactPhone = ""
            refundDeposit.RequestBody.bsno = ""
            refundDeposit.RequestBody.transId = ""
            refundDeposit.RequestBody.reasonCode = GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyReasonCode.Others
            refundDeposit.RequestBody.rejectReason = ""
            refundDeposit.RequestBody.remark1 = ""


            Dim response As ManageSecurityDeposit.RefundSecurityDepositResponse = proxy.RefundSecurityDeposit(refundDeposit)

            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then
                Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description _
                        & "|" & response.ResponseBody.responseMessage

            Else
                Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                    & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description _
                    & "|" & response.ResponseBody.responseMessage
            End If
            'Catch ex As Exception

            'End Try


        End Using

err:

        Return Err.Description
    End Function

    Public Function RegisterPayment(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                    ByVal msisdn As String, ByVal invoiceNo As String, ByVal paymethod As String, _
                                          ByVal amt As Double) As String
        On Error GoTo err
        Using proxy As New RegisterPayment.RegisterPaymentService

            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim regPay As New RegisterPayment.RegisterPaymentRequest

            regPay.RequestHeader = New RegisterPayment.RequestHeader

            regPay.RequestHeader.GeneralConsumerInformation = New RegisterPayment.GeneralConsumerInfoType
            regPay.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            regPay.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation() '"4c153812-8a13-4f48-81e2-e68fcbf48e39"
            regPay.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.RegisterPayment.CountryContentType.GHA
            regPay.RequestHeader.GeneralConsumerInformation.transactionID = ""

            regPay.RequestBody = New GhEpinDLL.RegisterPayment.requestBodyType
            regPay.RequestBody.msisdn = msisdn
            regPay.RequestBody.invoiceNumber = invoiceNo
            regPay.RequestBody.institutionId = ""
            regPay.RequestBody.invoiceSerial = ""
            regPay.RequestBody.invoiceType = ""
            regPay.RequestBody.paymentReference = ""

            Dim pay As New RegisterPayment.paymentDetailType
            pay.paymentMethod = CStr(paymethod)
            pay.amount = CDec(amt)
            pay.approvalCode = ""
            pay.bankAcctNumber = ""
            pay.bankCode = ""
            pay.cardBatchNumber = ""
            pay.cardNumber = ""
            pay.cardType = ""
            pay.chequeNumber = ""
            pay.remark = "Bill"
            pay.voucherNumber = ""
            regPay.RequestBody.paymentDetail = pay


            Dim response As RegisterPayment.RegisterPaymentResponse = proxy.RegisterPayment(regPay)

            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then
                Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description _
                        & "|" & response.ResponseBody.result & "|" & response.ResponseBody.authorizationNumber & "|" & response.ResponseBody.description

            Else
                Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                    & response.ResponseHeader.GeneralResponse.description
            End If

        End Using

err:
        Return Err.Description
    End Function

    Public Function ReversePayment(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, ByVal msisdn As String) As String
        On Error GoTo err
        ' 41.6:8011

        Using proxy As New ReversePayment.ReversePaymentService
            proxy.Url = url
            proxy.RequestEncoding = System.Text.Encoding.UTF8
            ' Use the WSE 3.0 security token class...
            'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
            Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

            proxy.RequestSoapContext.Security.Tokens.Clear()
            proxy.RequestSoapContext.Security.Tokens.Add(token)
            proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

            Dim revpay As New ReversePayment.ReversePaymentRequest

            revpay.RequestHeader = New ReversePayment.RequestHeader

            revpay.RequestHeader.GeneralConsumerInformation = New ReversePayment.GeneralConsumerInfoType()
            revpay.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
            revpay.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & correlation() '"4c153812-8a13-4f48-81e2-e68fcbf48e39"
            revpay.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.ReversePayment.CountryContentType.GHA
            revpay.RequestHeader.GeneralConsumerInformation.transactionID = ""

            revpay.requestBody = New ReversePayment.ReversePaymentRequestRequestBody
            revpay.requestBody.msisdn = msisdn
            revpay.requestBody.logID = ""
            revpay.requestBody.remark = "Bill"


            Dim response As ReversePayment.ReversePaymentResponse = proxy.ReversePayment(revpay)

            If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ReversePayment.StatusContentType.OK Then

                Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.codeType _
                    & "|" & response.ResponseHeader.GeneralResponse.description _
                    & "|" & response.responseBody.logID

            Else
                Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.codeType _
                    & "|" & response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.description

            End If


        End Using

err:
        Return Err.Description
    End Function
    Function correlation() As String
        On Error Resume Next
        Dim code As String = "4c153812-8a13-4f48-81e2-e68fcbf48e39"
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghifklmnopqrstuvwxyz0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 36
            Dim idx As Integer = r.Next(0, 60)
            sb.Append(s.Substring(idx, 1))
            code = sb.ToString()
        Next
        Return code
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region


End Class

