package org.o7planning.model;

import java.nio.charset.Charset;
import java.util.Properties;
import java.util.Random;

import javax.mail.*;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;

public class MailModel {
	public static void SendMail(String to,String messages) {	
        String from = "fixasset-system@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("[FA System] Request password");
           String newPassword = getAlphaNumericString(19);
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********ASSET CONTROL SYSTEM**********</div>\r\n" + 
           		"    <div>This is new password: <span style=\"color: red; font-style: bold; font-size: 1.5rem;\">" + newPassword + "</span></div>\r\n" + 
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and change password</div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
           UserModel.changingPassByEmail(to, newPassword);
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void confirmFA(String to,String messages, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM ASSET CONTROL SYSTEM**********</div>\r\n" + 
        		" <div>You have a request for approval from the fixed asset system regarding FA information: <span style='color: red; font-style: bold; font-size: 1.5rem;'> "+messages+"</span> </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check <a href=\"" + linkApprove + "\">" + linkApprove + "</a> </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void LOGremindMEA(String to,String messages, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM ASSET CONTROL SYSTEM**********</div>\r\n" + 
        		" <div>Dear MEA/PLAN PIC,</br>This is LOG dept.</br>I already input CDS information for some FA waiting disposal</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check <a href=\"" + linkApprove + "\">" + linkApprove + "</a> </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void MEAremindLOG(String to,String messages, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM ASSET CONTROL SYSTEM**********</div>\r\n" + 
        		" <div>Dear Logistic PIC,</br>This is MEA/PLAN dept.</br>Some FA waiting you input CDS information for disposal</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and input CDS infor <a href=\"" + linkApprove + "\">" + linkApprove + "</a> </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void createActualRequest(String to,String messages, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM ASSET CONTROL SYSTEM**********</div>\r\n" + 
        		" <div>Dear MEA/PLAN PIC, </br>GD already approve disposal request for some FA. Next step MEA create actual disposal</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check <a href=\"" + linkApprove + "\">" + linkApprove + "</a> </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void picConfirmFADepartment(String to, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM FIXED ASSET CONTROL SYSTEM**********</div>\r\n" + 
           		" <div>Dear PIC, </div>\r\n"+
        		" <div>Your department have some FA waiting Confirmation </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and confirm " + linkApprove + " </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void mgrConfirmFADepartment(String to, String linkApprove) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("CONFIRM INFORMATION FOR NEW FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>********FROM FIXED ASSET CONTROL SYSTEM**********</div>\r\n" + 
           		" <div>Dear MGR, </div>\r\n"+
        		" <div>Your department have some FA waiting Confirmation </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and confirm " + linkApprove + " </div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void SendDisposalReturn(String to,String messages) {
        String from = "fixasset-system@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        to = to.replaceAll(";", ",");
        //String[] strParts = to.split( "," );
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
//           for(int i = 0; i < strParts.length; i++) {
//        	   message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(strParts[i], true));
//           }
           message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(to, true));
           message.setSubject("[FA System] Return assets to department");
           message.setContent("<div>*******************************************************************</div>" + 
           		"    <h3>********ASSET CONTROL SYSTEM**********</h3>" + 
           		"    <div><span style=\"color: red; font-style: bold; font-size: 1rem;\">" + messages + "</span></div>" + 
           		"    <div></div>" + 
           		"    <p>-------------------------------------------------------------</p>" + 
           		"    <h5>Please recheck and no reply this message</h5>" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
        } catch (MessagingException mex) {
           mex.printStackTrace();
           System.out.println(mex);
        }
	}
	public static void SendApproveDeptRequest(String to,String messages) {
        String from = "fixasset-system@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        to = to.replaceAll(";", ",");
        //String[] strParts = to.split( "," );
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
//           for(int i = 0; i < strParts.length; i++) {
//        	   message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(strParts[i], true));
//           }
           message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(to, true));
           message.setSubject("[FA System] Approve request of department");
           message.setContent("<div>*******************************************************************</div>" + 
           		"    <h3>********ASSET CONTROL SYSTEM**********</h3>" + 
           		"    <div><span style=\"color: red; font-style: bold; font-size: 1rem;\">" + messages + "</span></div>" + 
           		"    <div></div>" + 
           		"    <p>-------------------------------------------------------------</p>" + 
           		"    <h5>Please no reply this message</h5>" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
        } catch (MessagingException mex) {
           mex.printStackTrace();
           System.out.println(mex);
        }
	}
	public static void SendMEAhavePending(String to,String messages) {
        String from = "fixasset-system@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        to = to.replaceAll(";", ",");
        //String[] strParts = to.split( "," );
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
//           for(int i = 0; i < strParts.length; i++) {
//        	   message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(strParts[i], true));
//           }
           message.addRecipients(Message.RecipientType.TO, InternetAddress.parse(to, true));
           message.setSubject("[FA System] Department request dispose FA");
           message.setContent("<div>*******************************************************************</div>" + 
           		"    <h3>********ASSET CONTROL SYSTEM**********</h3>" + 
           		"    <div><span style=\"color: red; font-style: bold; font-size: 1rem;\">" + messages + "</span></div>" + 
           		"    <div></div>" + 
           		"    <p>-------------------------------------------------------------</p>" + 
           		"    <h5>Please no reply this message</h5>" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
        } catch (MessagingException mex) {
           mex.printStackTrace();
           System.out.println(mex);
        }
	}
	static String getAlphaNumericString(int n) {
		byte[] array = new byte[256];
		new Random().nextBytes(array);
		String randomString = new String(array, Charset.forName("UTF-8"));
		StringBuffer r = new StringBuffer();
		String AlphaNumericString = randomString.replaceAll("[^A-Za-z0-9]", "");
		for (int k = 0; k < AlphaNumericString.length(); k++) {
			if (Character.isLetter(AlphaNumericString.charAt(k)) && (n > 0)
					|| Character.isDigit(AlphaNumericString.charAt(k)) && (n > 0)) {
				r.append(AlphaNumericString.charAt(k));
				n--;
			}
		}
		return r.toString();
	}
	public static void APTransferPlan(String to,String messages, String link) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("APPROVE TRANSFER FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>You have a request for <span style='background-color:yellow;'>approval transfer plan no: "+messages+"</span> from the fixed asset system</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check &nbsp;</div> " + link + "\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	
	public static void APTransferActual(String to,String messages, String link) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("APPROVE DATE ACTUAL TRANSFER FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>You have a request for <span style='background-color:yellow;'>approval transfer date received no: "+messages+"</span> from the fixed asset system</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check</div>&nbsp;" + link + "\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	
	public static void sendActual1(String to,String messages) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("APPROVE TRANSFER FIXED ASSET");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>You have a request for <span style='background-color:yellow;'>approval DATE RECEIVED no: "+messages+"</span> from the fixed asset system</div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check</div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	
	public static void WaitingList(String to,String messages) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("TRANSFER FIXED ASSET : LIST ACTUAL");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>You have a transfer request notice from the department that needs to fill in the date of receipt in <span style='background-color:yellow;'>TRANSFER =>LIST ACTUAL</span> </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check</div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	
	public static void FinishTransfer(String to,String messages) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("TRANSFER FIXED ASSET FINISH");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>Final approve finish Transfer no:<span style='background-color:yellow;'>"+messages+"</span> </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check</div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
	public static void reject(String to,String messages) {	
        String from = "fixedasset@local.canon-vn.com.vn";
        String host = "mail.cvn.canon.co.jp";
        Properties properties = System.getProperties();
        properties.setProperty("mail.smtp.host", host);
        properties.setProperty("mail.smtp.port", "2525");
        properties.setProperty("mail.transport.protocol", "smtp");  
        Session session = Session.getDefaultInstance(properties, null);
        try {
           MimeMessage message = new MimeMessage(session);
           message.setFrom(new InternetAddress(from));
           message.addRecipient(Message.RecipientType.TO, new InternetAddress(to));
           message.setSubject("TRANSFER FIXED ASSET : LIST ACTUAL");
         
           message.setContent("<div>*******************************************************************</div>\r\n" + 
           		"    <div>******************FROM ASSET CONTROL SYSTEM*******************</div>\r\n" + 
        		" <div>System anounce Transfer No: <span style='background-color:yellow;'>"+messages+"</span> was rejected </div>\r\n"+
     
           		"    <div></div>\r\n" + 
           		"    <div>-----------------------------------------------------------------------------------------------------</div>\r\n" + 
           		"    <div>Please login and check</div>\r\n" + 
           		"    <div>*******************************************************************</div>", "text/html");
           Transport.send(message);
         
        } catch (MessagingException mex) {
           mex.printStackTrace();
        }
	}
}
