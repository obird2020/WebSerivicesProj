Imports svclibrary
Imports GetSubscriber = GhEpinDLL.GetSubscriberInfo
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
Imports System.Data.SqlClient

<ComClass(clsEpin.ClassId, clsEpin.InterfaceId, clsEpin.EventsId)> _
Public Class clsEpin
    Dim ObjClsEventLog As ClsEventLog = New ClsEventLog()
    Dim success As Boolean = False
    Dim custtype As String = ""
    Dim defaultcred As New System.Net.Configuration.DefaultProxySection
    Dim corID As String = ""
    Dim pWd, dbName, server, names, ConString As String

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "4bf647c0-c9ae-426a-a891-8d7662f2da74"
    Public Const InterfaceId As String = "dc343daf-11be-4584-8092-27e667756ab9"
    Public Const EventsId As String = "6e1c7d16-63b2-4e4c-a943-238a69a552c6"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    Public Function RechargeSub(ByVal rechargeAmt As Double, ByVal MSISDN As String, ByVal PIN As String, _
                                ByVal SDMSISDN As String) As String
        Dim Client As New Service()
        Dim random As New Random()
        Dim myRandon As Int64 = random.[Next](500, 999999)
        Dim result = Client.RechargeSub(New RechargeSubRequest() With { _
             .Amount = Convert.ToDecimal(rechargeAmt * 10000), _
             .ClientID = 2, _
             .KeyCode = "DB670E4D42824030BF550E6484FC07BE", _
             .MSISDN = MSISDN, _
             .PIN = PIN, _
             .SDMSISDN = SDMSISDN, _
             .TransactionID = myRandon _
            })

        Return result.ReturnCode & "|" & result.TransactionID & "|" & result.Comment & "|" & result.Message
    End Function

    Public Function Transfer(ByVal value As Double, ByVal DestMsisdn As String, ByVal PIN As String, _
                                ByVal SrcMsisdn As String) As String

        Dim random As New Random()
        Dim myRandon As Int64 = random.[Next](500, 999999)
        Dim Client As New Service()

        Dim result = Client.TransferPayment(New TransferPaymentRequest() With { _
          .SrcMsisdn = SrcMsisdn, _
          .KeyCode = "DB670E4D42824030BF550E6484FC07BE", _
          .DestMsisdn = DestMsisdn, _
          .Value = Convert.ToDecimal(value * 10000), _
          .PIN = PIN, _
          .TransactionID = myRandon _
        })

        Return result.ReturnCode & "|" & result.TransactionID & "|" & result.Comment & "|" & result.Message
    End Function
    Public Function GetSubscriberInfo(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                      ByVal msisdn As String, ByVal typ As Integer) As String



        Dim webRequest As HttpWebRequest
        Dim reader As StreamReader
        Dim response As HttpWebResponse = Nothing
        Dim result As String
        Dim data As String = ""
        'Dim url As String = ""
        Dim xdoc As New XmlDocument
        Dim read As XmlTextReader
        result = ""
        'TextBox1.Text = ""
        Dim xml As String = ""

        xml = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:v1=""http://xmlns.tigo.com/GetSubscriberInfoRequest/V1"" xmlns:v3=""http://xmlns.tigo.com/RequestHeader/V3"" xmlns:v2=""http://xmlns.tigo.com/ParameterType/V2"" xmlns:cor=""http://soa.mic.co.af/coredata_1"">" _
                  & "<soapenv:Header xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">" _
                      & "<cor:debugFlag>true</cor:debugFlag>" _
                      & "<wsse:Security>" _
                          & "<wsse:UsernameToken>" _
                              & "<wsse:Username>{0}</wsse:Username>" _
                              & "<wsse:Password>{1}</wsse:Password> " _
                          & "</wsse:UsernameToken>" _
                      & "</wsse:Security>" _
                  & "</soapenv:Header>" _
                  & "<soapenv:Body> " _
                      & "<v1:GetSubscriberInfoRequest> " _
                          & "<v3:RequestHeader> " _
                              & "<v3:GeneralConsumerInformation>" _
                                  & "<v3:consumerID>{2}</v3:consumerID> " _
                                  & "<!--Optional:-->" _
                                  & "<v3:transactionID/> " _
                                  & "<v3:country>GHA</v3:country> " _
                                  & "<v3:correlationID>{4}</v3:correlationID> " _
                              & "</v3:GeneralConsumerInformation> " _
                          & "</v3:RequestHeader> " _
                          & "<v1:requestBody> " _
                              & "<v1:msisdn>{3}</v1:msisdn> " _
                              & "<v1:searchType>ALL</v1:searchType> " _
                          & "</v1:requestBody>" _
                      & "</v1:GetSubscriberInfoRequest> " _
                  & "</soapenv:Body> " _
              & "</soapenv:Envelope> "

        webRequest = DirectCast(System.Net.WebRequest.Create(url), HttpWebRequest)
        webRequest.Method = "POST"
        webRequest.UseDefaultCredentials = False
        'webRequest.Proxy = CType(Nothing, IWebProxy)

        'Added by martey to bypass Proxy on 25th Feb 2015
        Dim myProxy As WebProxy = New WebProxy()
        myProxy.Credentials = CredentialCache.DefaultCredentials
        webRequest.Proxy = myProxy
        corID = correlation()

        ' MsgBox(corID)
        webRequest.ContentType = "application/x-www-form-urlencoded"
        Dim postdata As String = String.Format(xml, user, pwd, consumerid, msisdn, "POS-" & corID)
        webRequest.ContentLength = postdata.Length
        webRequest.KeepAlive = True

        'MsgBox(postdata)

        Try
            Dim writer As New StreamWriter(webRequest.GetRequestStream(), System.Text.Encoding.ASCII)
            writer.Write(postdata)
            writer.Close()
            response = DirectCast(webRequest.GetResponse(), HttpWebResponse)
            'MsgBox("try catch")
        Catch ex As WebException

            Dim errResp As WebResponse = ex.Response
            If Not IsNothing(errResp) Then
                Using respStream As Stream = errResp.GetResponseStream()
                    reader = New StreamReader(respStream)
                    read = New XmlTextReader(reader)
                    read.WhitespaceHandling = WhitespaceHandling.Significant

                    While read.Read()
                        If Not String.IsNullOrEmpty(read.Value) Then
                            result &= read.Value & "|"
                        End If
                    End While

                    If Microsoft.VisualBasic.Right(result, 1) = "|" Then
                        result = Microsoft.VisualBasic.Left(result, Len(result) - 1)
                    End If
                    Dim vals() As String = Split(result, "|")

                    'MsgBox(ex.Message & "a")
               ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                    saveLog("GetSubscriberInfo", "500" & "|" & vals(3), corID)

                    Return "500" & "|" & vals(3)

                    Exit Function
                End Using
            Else
                Return ex.Message
                Exit Function
            End If
        End Try

        Select Case response.StatusCode

            Case HttpStatusCode.OK
                Dim xml_doc As New Xml.XmlDocument
                xml_doc.Load(webRequest.GetResponse().GetResponseStream())
                Dim nsMgr As New XmlNamespaceManager(xml_doc.NameTable)
                nsMgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/")
                nsMgr.AddNamespace("v11", "http://xmlns.tigo.com/GetSubscriberInfoResponse/V1")


                Dim customer As XmlNode = xml_doc.SelectSingleNode("/soapenv:Envelope/soapenv:Body/v11:GetSubscriberInfoResponse/v11:responseBody/v11:customer", nsMgr)
                Dim subscriber As XmlNode = xml_doc.SelectSingleNode("/soapenv:Envelope/soapenv:Body/v11:GetSubscriberInfoResponse/v11:responseBody/v11:subscriber", nsMgr)

                Dim final As String = innertext(customer, subscriber, msisdn)

                saveLog("GetSubscriberInfo", final, corID)

                Return final

                Exit Function

            Case HttpStatusCode.InternalServerError
                Return CStr(HttpStatusCode.InternalServerError)

            Case Else
                Return response.StatusDescription
        End Select





    End Function
    Function innertext(ByVal n As XmlNode, ByVal o As XmlNode, ByVal msisdn As String) As String
        On Error GoTo errhan
        Dim result As String = ""

        result &= "OK" & "|"
        result &= "getsubscriberinfo-1055-0000-S" & "|"
        result &= "The request has been processed successfully" & "|"
        result &= msisdn & "|"
        If Not (n("v12:firstName") Is Nothing) And Not (n("v12:middlename") Is Nothing) And Not (n("v12:lastName") Is Nothing) Then ' All 3
            result &= n("v12:firstName").InnerText & " " & n("v12:middlename").InnerText & " " & n("v12:lastName").InnerText & "|"
        ElseIf Not (n("v12:firstName") Is Nothing) And Not (n("v12:middlename") Is Nothing) Then
            result &= n("v12:firstName").InnerText & " " & n("v12:middlename").InnerText & "|"
        ElseIf Not (n("v12:firstName") Is Nothing) And Not (n("v12:lastName") Is Nothing) Then
            result &= n("v12:firstName").InnerText & " " & n("v12:lastName").InnerText & "|"
        ElseIf Not (n("v12:firstName") Is Nothing) Then
            result &= n("v12:firstName").InnerText & "|"
        ElseIf Not (n("v12:name") Is Nothing) Then
            result &= n("v12:name").InnerText & "|"
        ElseIf Not (n("v12:lastName") Is Nothing) Then
            result &= n("v12:lastName").InnerText & "|"
        End If
        If Not (n("v12:clientCode") Is Nothing) Then
            result &= n("v12:clientCode").InnerText & "|"
        Else
            result &= "|"
        End If
        If Not (o("v12:customerType") Is Nothing) Then
            result &= o("v12:customerType").InnerText & "|"
        Else
            result &= "|"
        End If
        If Not (o("v12:mainProductID") Is Nothing) Then
            result &= o("v12:mainProductID").InnerText & "|"
        Else
            result &= "|"
        End If
        If Not (o("v12:initialCredit") Is Nothing) Then
            result &= o("v12:initialCredit").InnerText & "|"
        Else
            result &= "|"
        End If
        If Not (o("v12:brandId") Is Nothing) Then
            result &= o("v12:brandId").InnerText

        End If

        Return result

errhan:
        result &= "|"
        Resume Next
    End Function
    Public Function GetInvoiceDetail(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                      ByVal typ As Integer, ByVal msisdn As String, ByVal accountcode As String) As String

        Using proxy As New GetInvoiceDetail.GetInvoiceDetailService
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                ' Use the WSE 3.0 security token class...
                'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim getInvoice As New GetInvoiceDetail.GetInvoiceDetailRequest

                getInvoice.RequestHeader = New GetInvoiceDetail.RequestHeader

                corID = correlation()
                MsgBox(corID & "invde")
                getInvoice.RequestHeader.GeneralConsumerInformation = New GetInvoiceDetail.GeneralConsumerInfoType()
                getInvoice.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                getInvoice.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
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

                getInvoice.requestBody.TotalRowNum = "0"
                getInvoice.requestBody.BeginRowNum = "0"
                getInvoice.requestBody.FetchRowNum = "0"


                Dim response As GetInvoiceDetail.GetInvoiceDetailResponse = proxy.GetInvoiceDetail(getInvoice)

                Dim invoice() As GhEpinDLL.GetInvoiceDetail.invoiceDetailType

                If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.GetInvoiceDetail.StatusContentType.OK Then
                    invoice = response.responseBody.invoiceDetailList
                    If invoice.Length > 0 Then

                        Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                                & "|" & response.ResponseHeader.GeneralResponse.description _
                                & "|" & invoice(0).acctCode & "|" & invoice(0).acctId & "|" & invoice(0).BillCycleId & "|" & invoice(0).custCode _
                                & "|" & invoice(0).custId & "|" & invoice(0).transType & "|" & invoice(0).invoiceId & "|" & invoice(0).invoiceNo _
                                & "|" & invoice(0).InvoiceAmount & "|" & invoice(0).invoiceDate & "|" & invoice(0).OpenAmount & "|" & invoice(0).dueDate _
                                & "|" & invoice(0).SettleDate & "|" & invoice(0).currencyId & "|" & invoice(0).subId

                    Else
                        Return response.ResponseHeader.GeneralResponse.description
                    End If

                Else
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description
                End If

                saveLog("GetInvoiceDetail", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description, corID)

            Catch ex As Exception
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                saveLog("error", "wew", corID)
                Return ex.Message
            End Try


        End Using

    End Function

    Public Function GetSecurityDepositDetails(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                              ByVal invoiceid As String, ByVal accountid As String, flag As Integer) As String()

        Dim i As Integer = 0
        Dim x As Integer = 0
        Dim errorlist(10) As String
        Using proxy As New GetSecurityDepositDetails.GetSecurityDepositDetailsService
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                ' Use the WSE 3.0 security token class...
                'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim getdeposit As New GetSecurityDepositDetails.GetSecurityDepositDetailsRequest

                getdeposit.RequestHeader = New GetSecurityDepositDetails.RequestHeader

                corID = correlation()
                MsgBox(corID)
                getdeposit.RequestHeader.GeneralConsumerInformation = New GetSecurityDepositDetails.GeneralConsumerInfoType()
                getdeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                getdeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
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
                    Dim arrayvalues(deposit.Length - 1) As String


                    If deposit.Length > 0 Then
                        For i = 0 To deposit.GetUpperBound(0)
                            arrayvalues(i) = response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.description _
                                & "|" & deposit(i).AcctId & "|" & deposit(i).Msisdn & "|" & deposit(i).SubId & "|" & deposit(i).DepositType _
                                & "|" & deposit(i).InvoiceAmount & "|" & deposit(i).OpenAmount _
                                & "|" & deposit(i).Status & "|" & deposit(i).EntryDate

                            If depositList.Length > 0 Then
                                arrayvalues(i) = arrayvalues(i) & "|" & depositList(0).PaymentMethod & "|" & depositList(0).Amount
                            End If
                        Next


                        Return arrayvalues

                    Else
                        errorlist(0) = response.ResponseHeader.GeneralResponse.description
                        Return errorlist
                    End If

                Else
                    errorlist(0) = response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description

                    Return errorlist

                End If

                saveLog("GetSecurityDepositDetails", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description, corID)

            Catch ex As Exception
                errorlist(0) = ex.Message
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)

                Return errorlist
            End Try
        End Using

        MsgBox("error")
    End Function

    Public Function ManageSecurityDeposit(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                          ByVal msisdn As String, ByVal accountnumber As String, ByVal paymethod As Integer, _
                                          ByVal amt As Double, ByVal chargecustomer As Boolean, ByVal checknum As String) As String


        Using proxy As New ManageSecurity.ManageSecurityDeposit
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                ' Use the WSE 3.0 security token class...
                'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim manageDeposit As New ManageSecurity.SecurityDeposit

                manageDeposit.RequestHeader = New ManageSecurityDeposit.RequestHeader

                corID = correlation()
                manageDeposit.RequestHeader.GeneralConsumerInformation = New ManageSecurityDeposit.GeneralConsumerInfoType
                manageDeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                manageDeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
                manageDeposit.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.ManageSecurityDeposit.CountryContentType.GHA
                manageDeposit.RequestHeader.GeneralConsumerInformation.transactionID = ""

                manageDeposit.RequestBody = New ManageSecurityDeposit.SecurityDepositRequestBody
                manageDeposit.RequestBody.msisdn = msisdn
                manageDeposit.RequestBody.accountNumber = accountnumber
                manageDeposit.RequestBody.chargeCustomer = chargecustomer

                Dim pay As New ManageSecurityDeposit.paymentDetail
                pay.paymentMethod = CType(paymethod, GhEpinDLL.ManageSecurityDeposit.paymentMethod)
                pay.paymentAmount = CDec(amt)
                pay.chequeNumber = checknum
                pay.bankCode = ""
                pay.cardNumber = ""
                pay.cardType = ""
                pay.cardBatchNumber = ""
                pay.approvalCode = ""
                pay.remark = ""

                manageDeposit.RequestBody.paymentDetail = pay

                Dim response As ManageSecurityDeposit.SecurityDepositResponse = proxy.SecurityDeposit(manageDeposit)

                If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description _
                            & "|" & response.ResponseBody.transactionReference & "|" & response.ResponseBody.depositAmount

                Else
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description
                End If

                saveLog("ManageSecurityDeposit", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description, corID)

            Catch ex As Exception
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                Return "500" & "|" & ex.Message

            End Try
        End Using

    End Function

    Public Function RefundSecurityDeposit(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                          ByVal msisdn As String, ByVal iscustomer As Integer, typ As Integer, ByVal accountcodeid As String, ByVal reftyp As Integer, _
                                          ByVal amt As Double, ByVal reason As Integer, ByVal bankaccttype As Integer) As String

        Using proxy As New ManageSecurity.ManageSecurityDeposit
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                ' Use the WSE 3.0 security token class...
                'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim refundDeposit As New ManageSecurity.RefundSecurityDeposit

                refundDeposit.RequestHeader = New ManageSecurityDeposit.RequestHeader

                corID = correlation()
                refundDeposit.RequestHeader.GeneralConsumerInformation = New ManageSecurityDeposit.GeneralConsumerInfoType
                refundDeposit.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                refundDeposit.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
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
                refundDeposit.RequestBody.amount = CDec(amt)
                refundDeposit.RequestBody.bankAcctType = CType(bankaccttype, GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyBankAcctType)
                refundDeposit.RequestBody.bankBranchNumber = ""
                refundDeposit.RequestBody.bankAcctNo = ""
                refundDeposit.RequestBody.contactPhone = ""
                refundDeposit.RequestBody.bsno = ""
                refundDeposit.RequestBody.transId = ""
                refundDeposit.RequestBody.reasonCode = CType(reason, GhEpinDLL.ManageSecurityDeposit.RefundSecurityDepositRequestBodyReasonCode)
                refundDeposit.RequestBody.rejectReason = ""
                refundDeposit.RequestBody.remark1 = ""


                Dim response As ManageSecurityDeposit.RefundSecurityDepositResponse = proxy.RefundSecurityDeposit(refundDeposit)

                If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description _
                            & "|" & response.ResponseBody.responseMessage

                Else
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & "|" & response.ResponseHeader.GeneralResponse.codeType & "|" & response.ResponseHeader.GeneralResponse.description _
                        & "|" & response.ResponseBody.responseMessage
                End If

                saveLog("RefundSecurityDeposit", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description, corID)

            Catch ex As Exception
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                Return "500" & "|" & ex.Message

            End Try

        End Using

    End Function

    Public Function RegisterPayment(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, _
                                    ByVal msisdn As String, ByVal invoiceNo As String, ByVal paymethod As String, _
                                          ByVal amt As Double, ByVal checknum As String) As String

        Using proxy As New RegisterPayment.RegisterPaymentService
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim regPay As New RegisterPayment.RegisterPaymentRequest

                regPay.RequestHeader = New RegisterPayment.RequestHeader

                corID = correlation()
                regPay.RequestHeader.GeneralConsumerInformation = New RegisterPayment.GeneralConsumerInfoType
                regPay.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                regPay.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
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
                pay.remark = "RMS Payment"
                pay.chequeNumber = checknum
                regPay.RequestBody.paymentDetail = pay


                Dim response As RegisterPayment.RegisterPaymentResponse = proxy.RegisterPayment(regPay)

                If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ManageSecurityDeposit.StatusContentType.OK Then

                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.description _
                            & "|" & response.ResponseBody.result & "|" & response.ResponseBody.authorizationNumber & "|" & response.ResponseBody.description

                Else
                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & response.ResponseHeader.GeneralResponse.description
                End If

                saveLog("RegisterPayment", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description & "|" & response.ResponseBody.authorizationNumber, corID)

            Catch ex As Exception
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                Return "500" & "|" & ex.Message

            End Try
        End Using

    End Function

    Public Function ReversePayment(ByVal url As String, ByVal consumerid As String, ByVal user As String, ByVal pwd As String, ByVal msisdn As String, _
                                   ByVal logid As String) As String

        Using proxy As New ReversePayment.ReversePaymentService
            Try
                proxy.Url = url
                proxy.RequestEncoding = System.Text.Encoding.UTF8
                defaultcred.UseDefaultCredentials = False
                defaultcred.Enabled = False
                ' Use the WSE 3.0 security token class...
                'Dim token As New UsernameToken("test_mw_rhema", "t35tmwrh3m@", PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW test
                Dim token As New UsernameToken(user, pwd, PasswordOption.SendPlainText) ',live_mw_rhema,L1v3rh3m@MW live

                'Added by martey to bypass Proxy on 25th Feb 2015
                Dim myProxy As WebProxy = New WebProxy()
                myProxy.Credentials = CredentialCache.DefaultCredentials
                proxy.Proxy = myProxy

                proxy.RequestSoapContext.Security.Tokens.Clear()
                proxy.RequestSoapContext.Security.Tokens.Add(token)
                proxy.RequestSoapContext.Security.Timestamp.TtlInSeconds = 360

                Dim revpay As New ReversePayment.ReversePaymentRequest

                revpay.RequestHeader = New ReversePayment.RequestHeader

                corID = correlation()
                revpay.RequestHeader.GeneralConsumerInformation = New ReversePayment.GeneralConsumerInfoType()
                revpay.RequestHeader.GeneralConsumerInformation.consumerID = consumerid
                revpay.RequestHeader.GeneralConsumerInformation.correlationID = "POS-" & corID
                revpay.RequestHeader.GeneralConsumerInformation.country = GhEpinDLL.ReversePayment.CountryContentType.GHA
                revpay.RequestHeader.GeneralConsumerInformation.transactionID = ""

                revpay.requestBody = New ReversePayment.ReversePaymentRequestRequestBody
                revpay.requestBody.msisdn = msisdn
                revpay.requestBody.logID = logid
                revpay.requestBody.remark = "Reverse Payment"


                Dim response As ReversePayment.ReversePaymentResponse = proxy.ReversePayment(revpay)

                If response.ResponseHeader.GeneralResponse.status = GhEpinDLL.ReversePayment.StatusContentType.OK Then

                    Return response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code _
                        & "|" & response.ResponseHeader.GeneralResponse.codeType _
                        & "|" & response.responseBody.logID _
                        & "|" & response.ResponseHeader.GeneralResponse.description

                Else
                    Return response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.codeType _
                        & "|" & response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.description


                End If

                saveLog("ReversePayment", response.ResponseHeader.GeneralResponse.status & "|" & response.ResponseHeader.GeneralResponse.code & "|" & response.ResponseHeader.GeneralResponse.description, corID)

            Catch ex As Exception
                ObjClsEventLog.WriteToEventLog("RMS-CBS", ex.Source & vbCrLf & ex.StackTrace, vbCrLf & vbCrLf & ex.Message, success)
                Return "500" & "|" & ex.Message

            End Try

        End Using

    End Function
    Function correlation() As String
        On Error Resume Next
        Dim code As String = "4c153812-8a13-4f48-81e2-e68fcbf48e39"
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghifklmnopqrstuvwxyz0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 16
            Dim idx As Integer = r.Next(0, 60)
            sb.Append(s.Substring(idx, 1))
            code = sb.ToString()
            corID = code
        Next

        Return code
    End Function
    Sub saveLog(ByVal proc As String, ByVal resp As String, ByVal cor As String)
        Call conInfo()
        MsgBox("saving")
        Try
            Using con As New SqlConnection(ConString)
                MsgBox(ConString, "Connection String2")
                con.Open()
                cor = "POS-" & cor
                Dim sql As String = "Insert into Rhema_Servicelog(Process,CorrelationID,Response) Values('" & proc & "','" & cor & "','" & resp & "')"
                Dim cm As New SqlCommand(sql, con)
                cm.ExecuteNonQuery()

                con.Close()
            End Using
        Catch EX As Exception
            MsgBox(EX.Message & "Logging error")
        End Try
    End Sub
    Public Sub conInfo()
        Try
            Dim fil As String = System.AppDomain.CurrentDomain.BaseDirectory & "AddIns\msysBq.frt"
            Dim x As Integer = -1
            Dim ConnectionInfo(10) As String

            Dim objReader As New System.IO.StreamReader(fil, System.Text.Encoding.UTF7)
            Do While objReader.Peek <> -1
                x += 1
                ConnectionInfo(x) = objReader.ReadLine
            Loop
            objReader.Close()

            server = ConnectionInfo(0)
            dbName = ConnectionInfo(2)
            ' pWd = Decode(ConnectionInfo(3))
            pWd = Decode(ConnectionInfo(3))
            'shop = ConnectionInfo(8)
            ConString = "Data Source=" & server & ";" & "Initial Catalog=" & dbName & ";User Id=rmssa;Password=" & pWd
            MsgBox(ConString, "Connection String1")
        Catch ex As Exception
            MsgBox(ex.Message, "Connection Error")
        End Try
    End Sub
    Function Decode(ByVal pssword As String) As String
        Dim R As Short
        Dim nmbr As String
        Dim txt As String
        Dim eTtxt As Short
        nmbr = ""
        txt = ""
        For R = 1 To Len(pssword)
            If IsNumeric(Mid(pssword, R, 1)) Then
                txt = txt & Mid(pssword, R, 1)
            End If
        Next

        For R = 1 To Len(txt) Step 3
            eTtxt = CShort(Mid(txt, R, 3))
            nmbr = nmbr & Chr(eTtxt)
        Next
        Return nmbr
    End Function

End Class


