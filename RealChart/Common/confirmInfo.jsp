<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@include file="/templates/inc/Subheader.jsp"%>
<head>
	<title>Printed from FA control system</title>
</head>
<%@page import="org.o7planning.bean.TotalBean"%>
<%@page import="java.util.ArrayList"%>
<%@page import="java.util.*"%>
<%@taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<%@ taglib prefix="form" uri="http://www.springframework.org/tags/form"%>
<%@page import="java.io.File"%>
<%@page import="org.o7planning.model.TotalModel"%>
<%@page import="java.text.DecimalFormat" %>
<meta name="viewport" content="width=device-width, minimum-scale=0.1">
<%
	ArrayList<TotalBean> model = (ArrayList<TotalBean>) request.getAttribute("danhsach");
	String mDept = (String) request.getAttribute("mDept");
	String mMea = (String) request.getAttribute("mMea");
	String pMea = (String) request.getAttribute("pMea");
	List<String> lst = (List<String>) request.getAttribute("danhsachPIC");
	String user = (String) request.getSession().getAttribute("username");
	String original = (String) request.getAttribute("originalCost_center");
	
	ArrayList<TotalBean> trans_his = (ArrayList<TotalBean>) request.getAttribute("transfer_history");
	boolean check =false;
	for(int i=0;i<lst.size();i++)
	{
		if( user.equals(lst.get(i)) )
		{
			check=true;
		}
	}
	DecimalFormat formatter = new DecimalFormat("#,###.##");
	
%>
    <%@page import="java.io.File"%>
    <%@page import="java.io.IOException"%>
    <%@page import="java.awt.image.BufferedImage"%>
    <%@page import="javax.imageio.ImageIO"%>
    <%@page import="java.io.ByteArrayOutputStream"%>

    <%@page import="java.math.BigInteger"%>
    <%@page import="javax.xml.bind.DatatypeConverter"%>
    <%@page import="java.awt.image.BufferedImage"%>




<script type="text/javascript">


	function deptapprove(asset_no)
	{
		if(confirm("Are you sure you want to approve ?"))
			{
				$.ajax({
					type : "POST",
					url : "deptapprove",
					data : {
						'asset_no' : asset_no,
					},
					success: function (response){
					 alert("Approve Success !");
					 location.reload();
					},
				 	 error: function (error){
				  		alert("Approve Error !");
				  		}
	
				});
			}else
				{
				return false;
				}
			
		
		
	}

	function meaapprove(asset_no)
	{
		if(confirm("Are you sure you want to approve ?"))
		{
			$.ajax({
				type : "POST",
				url : "meaapprove",
				data : {
					'asset_no' : asset_no,
				},
				success: function (response){
				 alert("Approve Success !");
				 location.reload();
				},
			 	 error: function (error){
			  		alert("Approve Error !");
			  		}

			});
		}else
			{
			return false;
			}
		

	}
	
	
	function picapprove(asset_no)
	{
		if(confirm("Are you sure you want to approve ?"))
		{
			$.ajax({
				type : "POST",
				url : "picapprove",
				data : {
					'asset_no' : asset_no,
				},
				success: function (response){
				 alert("Approve Success !");
				 location.reload();
				},
			 	 error: function (error){
			  		alert("Approve Error !");
			  		}

			});
		}else
			{
			return false;
			}
		

	}
	function picmeaapprove(asset_no)
	{
		if(confirm("Are you sure you want to approve ?"))
		{
			$.ajax({
				type : "POST",
				url : "picmeaapprove",
				data : {
					'asset_no' : asset_no,
				},
				success: function (response){
				 alert("Approve Success !");
				 location.reload();
				},
			 	 error: function (error){
			  		alert("Approve Error !");
			  		}

			});
		}else
			{
			return false;
			}
		

	}
	
	
</script>
<style>
img.img-responsive{
	max-height: 50vh;
}
	.infor-title{
		margin-bottom: 0.5rem;
	}
	.infor-value{
		font-weight: bold;
		font-style: italic;
		margin-bottom: 0.5rem;
	}
	table tr>th{
	color: white;
	text-transform: capitalize;
}
table tr>th, table tr>td{
	text-align:center !important;
	vertical-align: middle !important;
	border: 1px solid #6886f9 !important;
}
.txt-security {
    font-weight: 800 !important;
    border: 2px solid red;
    font-size: 16px;
    padding: 0.3rem 0.5rem;
    border-radius: 0.5rem;
    text-align: right;
    width: 220px;
    float: right;
    letter-spacing: 0.2rem;
}
.txt-header {
    font-size: 2.5rem;
    display: flex;
    justify-content: center;
    color: blue;
    font-weight: 700;
    margin: 0rem 0rem 1rem 0rem;
    text-align: center;
    width: 100%;
}
.font-15{
	font-size: 1.1rem;
}
.not-print{
	float: left;
}
@media print { 
    .not-print { 
       visibility: hidden; 
    } 
} 
</style>
<!-- main code -->
<!-- Begin Page Content -->
						<%if (model != null) {
	for (TotalBean item : model) {%> 
<div class="container-fluid">

	<!-- Page Heading -->

	<!-- DataTales Example -->
	<div class=" mb-4" id="printDiv">
		<div class="py-0">
			<div class="not-print">
				<button class="btn btn-sm btn-secondary" onclick="window.history.go(-1); return false;">Back</button>
				<a class="btn btn-sm btn-primary" href="<%=request.getContextPath()%>/Home">Home</a>
			</div>
			<div class="text-danger txt-security">R2 CONFIDENTIAL</div>
		</div>
		<div class="">
			<div class="txt-header">CONFIRMATION FOR NEW FIXED ASSET</div>
			<div class="row mb-5">
				<div class="col-xs-0 col-sm-1 col-lg-2">
				</div>
				<div class="col-xs-0 col-sm-10 col-lg-8">
					<%
				    		String old_fa = item.getName_old_fa();
							if(Character.isDigit(old_fa.charAt(0))){//bắt đầu là số thì ảnh lấy theo tên fa cũ
								try{
					          	      String imgName="D:\\Anh\\"+item.getName_old_fa()+".JPG";
					          	      BufferedImage bImage = ImageIO.read(new File(imgName));//give the path of an image
					          	        ByteArrayOutputStream baos = new ByteArrayOutputStream();
					          	        ImageIO.write( bImage, "jpg", baos );
					          	        baos.flush();
					          	        byte[] imageInByteArray = baos.toByteArray();
					          	        baos.close();                                   
					          	        String b64 = DatatypeConverter.printBase64Binary(imageInByteArray);
					          	        %>
					          	        <img  class="img-responsive" src="data:image/jpg;base64, <%=b64%>" width="100%" height="auto"/>                            
					          	        <% 
					          	}catch(IOException e){
					          		%>
				          	        <img  class="img-responsive" src="<%=request.getContextPath() %>/templates/img/no_image.jpg" width="100%" height="auto"/>                            
				          	     	<% 
					          	} 
							}else{//còn lại lấy theo tên fa mới
								try{
					          	      String imgName="D:\\Anh\\"+item.getAsset_no()+".JPG";
					          	      BufferedImage bImage = ImageIO.read(new File(imgName));//give the path of an image
					          	        ByteArrayOutputStream baos = new ByteArrayOutputStream();
					          	        ImageIO.write( bImage, "jpg", baos );
					          	        baos.flush();
					          	        byte[] imageInByteArray = baos.toByteArray();
					          	        baos.close();                                   
					          	        String b64 = DatatypeConverter.printBase64Binary(imageInByteArray);
					          	        %>
					          	        <img  class="img-responsive" src="data:image/jpg;base64, <%=b64%>" width="100%" height="auto"/>                            
					          	        <% 
					          	    }catch(IOException e){
					          	    	%>
					          	        <img  class="img-responsive" src="<%=request.getContextPath() %>/templates/img/no_image.jpg" width="100%" height="auto"/>                            
					          	     	<% 
					          	    } 
							}
				            
    %>
				</div>
				<div class="col-xs-0 col-sm-1 col-lg-2">
					
				</div>
			</div>
			
			<div class="row font-15">
				<div class="col-2 text-right infor-title">
					FA Number:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getAsset_no() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Old FA:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getName_old_fa() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					FA Name:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getAsset_nm() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Description:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getNote1() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Budget code:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getCapital_budget_code() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Original cost(USD):
				</div>
				<div class="col-4 text-left infor-value">
					<%=formatter.format(item.getAcq_val_comp()) %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Class code:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getClass_cd() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Start using date:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getOpe_date().substring(0, 4) + "-" + item.getOpe_date().substring(4, 6) + "-" + item.getOpe_date().substring(6, 8) %>
				</div>
				
				<div class="col-2 text-right infor-title">
					FA Location:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getLocation_nm() %>
				</div>
				
				<div class="col-2 text-right infor-title">
					Cost Center:
				</div>
				<div class="col-4 text-left infor-value">
					<%=item.getCost_center() + " - " + item.getCost_center_name() %>
				</div>
				<div class="col-2 text-right infor-title">
					Original Cost center:
				</div>
				<%
					if(original != null && !original.equals("")){
						%>
						<div class="col-4 text-left infor-value">
							<%=original %>
						</div>
						<%
					}else{
						%>
						<div class="col-4 text-left infor-value">
							<%=item.getCost_center() + " - " + item.getCost_center_name() %>
						</div>
						<%
					}
				%>
				<%
					if(item.getCds_no() != null){
						%>
							<div class="col-2 text-right infor-title">
								CDS Number:
							</div>
							<div class="col-4 text-left infor-value">
								<%=item.getCds_no()%>
							</div>
						<%
					}
				%>
				
				<%
					if(item.getCds_location() != null){
						%>
							<div class="col-2 text-right infor-title">
								CDS Location:
							</div>
							<div class="col-4 text-left infor-value">
								<%=item.getCds_location()%>
							</div>
						<%
					}
				%>
				
				<div class="col-2 text-right infor-title">
					FA Structure:
				</div>
				<div class="col-4 text-left infor-value">
					<%
						if(Character.isDigit(item.getName_old_fa().charAt(0))){
							%>
							<form action="<%=request.getContextPath()%>/downloadFileOpen"
		method="get" enctype="multipart/form-data" id="downloadForm">
				<input type="hidden" class="w-auto" name="file_name" value="D:/FA structure/<%=item.getName_old_fa() %>.xlsx"/>
				<input type="submit" class="w-auto btn btn-link fon-15" value="<%=item.getName_old_fa() %>.xlsx"/>
					</form>
							<%
						}else{
							%>
							<form action="<%=request.getContextPath()%>/downloadFileOpen"
		method="get" enctype="multipart/form-data" id="downloadForm">
				<input type="hidden" class="w-auto" name="file_name" value="D:/FA structure/<%=item.getAsset_no() %>.xlsx"/>
				<input type="submit" class="w-auto btn btn-link font-15" value="<%=item.getAsset_no() %>.xlsx"/>
					</form>
							<%
						}
					%>
					
				</div>
				<div class="col-12">
				</div>
				<div class="col-2 text-right infor-title">
					Borrowing sheet:
				</div>
				<div class="col-4 text-left infor-value">
					<%
						if(Character.isDigit(item.getName_old_fa().charAt(0))){
							%>
							<form action="<%=request.getContextPath()%>/downloadFileOpen"
		method="get" enctype="multipart/form-data" id="downloadForm">
				<input type="hidden" class="w-auto" name="file_name" value="D:/Borrowing sheet/<%=item.getName_old_fa() %>.pdf"/>
				<input type="submit" class="w-auto btn btn-link fon-15" value="<%=item.getName_old_fa() %>.pdf"/>
					</form>
							<%
						}else{
							%>
							<form action="<%=request.getContextPath()%>/downloadFileOpen"
		method="get" enctype="multipart/form-data" id="downloadForm">
				<input type="hidden" class="w-auto" name="file_name" value="D:/Borrowing sheet/<%=item.getAsset_no() %>.pdf"/>
				<input type="submit" class="w-auto btn btn-link font-15" value="<%=item.getAsset_no() %>.pdf"/>
					</form>
							<%
						}
					%>
					
				</div>
			
			</div>
			<div class="row my-4">
				<div class="col-sm-12 font-weight-bold font-italic">
					Confirmation flow
				</div>
				<div class="col-sm-12">
					<table class="table table-dark w-100">
						<thead>
							<tr>
								<td>Step</td>
								<td>Acknowledger</td>
								<td>Submission</td>
								<td>Date</td>
								<td>Status</td>
							</tr>
						</thead>
						<tbody>
							<tr>
								<td>1</td>
								<td>MEA/PLAN upload data</td>
								<%
									String userAndate1 = TotalModel.getMEAuploadFile(item.getUpdated_by());
									String dateStr1 = item.getUpdated_date();
									%>
										<td><%=userAndate1 %></td>
										<td><%=dateStr1%></td>
										<td>Confirmed</td>
									<%
								%>
							</tr>
							<tr>
								<td>2</td>
								<td>PIC Department</td>
								<%
									if(item.getPicdept() != null){
										String userAndate = TotalModel.getUserInfo(item.getPicdept());
										String dateStr = userAndate.split("&")[1];
										%>
											<td><%=userAndate.split("&")[0] %></td>
											<td><%=dateStr.substring(0, 4) + "-" + dateStr.substring(4, 6) + "-" + dateStr.substring(6, 8) %></td>
											<td>Confirmed</td>
										<%
									}else{
										if(check == true){
											%>
											<td></td>
											<td></td>
											<td><button class="btn btn-sm btn-primary" onclick="picapprove('<%=item.getAsset_no()%>')">Confirm</button></td>
											<%
										}else{
											%>
											<td></td>
											<td></td>
											<td>Waiting confirm</td>
											<%
										}
									}
								%>
							</tr>
							<tr>
								<td>3</td>
								<td>MGR Department</td>
								<%
									if(item.getMgrdept() != null){
										String userAndate = TotalModel.getUserInfo(item.getMgrdept());
										String dateStr = userAndate.split("&")[1];
										%>
											<td><%=userAndate.split("&")[0] %></td>
											<td><%=dateStr.substring(0, 4) + "-" + dateStr.substring(4, 6) + "-" + dateStr.substring(6, 8) %></td>
											<td>Confirmed</td>
										<%
									}else{
										if(mDept.contains(user + ",") && item.getPicdept() != null){
											%>
											<td></td>
											<td></td>
											<td><button class="btn btn-sm btn-primary" onclick="deptapprove('<%=item.getAsset_no()%>')">Confirm</button></td>
											<%
										}else{
											%>
											<td></td>
											<td></td>
											<td>Waiting confirm</td>
											<%
										}
									}
								%>
							</tr>
							<tr>
								<td>4</td>
								<td>PIC MEA/PLAN</td>
								<%
									if(item.getPicmea() != null){
										String userAndate = TotalModel.getUserInfo(item.getPicmea());
										String dateStr = userAndate.split("&")[1];
										%>
											<td><%=userAndate.split("&")[0] %></td>
											<td><%=dateStr.substring(0, 4) + "-" + dateStr.substring(4, 6) + "-" + dateStr.substring(6, 8) %></td>
											<td>Confirmed</td>
										<%
									}else{
										if(pMea.contains(user + ",") && item.getMgrdept() != null){
											%>
											<td></td>
											<td></td>
											<td><button class="btn btn-sm btn-primary" onclick="picmeaapprove('<%=item.getAsset_no()%>')">Confirm</button></td>
											<%
										}else{
											%>
											<td></td>
											<td></td>
											<td>Waiting confirm</td>
											<%
										}
									}
								%>
							</tr>
							<tr>
								<td>5</td>
								<td>MGR MEA/PLAN</td>
								<%
									if(item.getMgrmea() != null){
										String userAndate = TotalModel.getUserInfo(item.getMgrmea());
										String dateStr = userAndate.split("&")[1];
										%>
											<td><%=userAndate.split("&")[0] %></td>
											<td><%=dateStr.substring(0, 4) + "-" + dateStr.substring(4, 6) + "-" + dateStr.substring(6, 8) %></td>
											<td>Confirmed</td>
										<%
									}else{
										if(mMea.contains(user + ",") && item.getPicmea() != null){
											%>
											<td></td>
											<td></td>
											<td><button class="btn btn-sm btn-primary" onclick="meaapprove('<%=item.getAsset_no()%>')">Confirm</button></td>
											<%
										}else{
											%>
											<td></td>
											<td></td>
											<td>Waiting confirm</td>
											<%
										}
									}
								%>
							</tr>
						</tbody>
					</table>
				</div>
			</div>	
			<div class="row">
				<div class="col-sm-12">
					<table class="table table-dark w-100">
						<thead>
							<tr>
								<th colspan="12" class="font-weight-bold font-italic text-uppercase" style="font-size: 2rem !important">Changing History</th>
							</tr>
							<tr>
								<th rowspan="3">No</th>
								<th colspan="8">Changing points</th>
								<th rowspan="3">Checked by PIC of MEA/PLAN</th>
								<th rowspan="3">Approved by MGR of MEA/PLAN</th>
								<th rowspan="3">Changing date</th>
							</tr>
							<tr>
								<th colspan="2">Cost center</th>
								<th colspan="2">FA Name</th>
								<th colspan="2">Class code</th>
								<th colspan="2">Capital Budget Code</th>
							</tr>
							<tr>
								<th>Old</th>
								<th>New</th>
								<th>Old</th>
								<th>New</th>
								<th>Old</th>
								<th>New</th>
								<th>Old</th>
								<th>New</th>
							</tr>
						</thead>
						<tbody>
							<%
								if(trans_his == null || trans_his.size() == 0){
									%>
										<tr><td colspan="13">No history</td></tr>
									<%
								}else{
									int k = 1;
									for(int i = 0; i < trans_his.size(); i++){
										TotalBean item2 = trans_his.get(i);
											if(item2.getUpdated_cost_center() != null || item2.getUpdated_asset_nm() != null
													|| item2.getUpdated_class_cd() != null || item2.getUpdated_capital_budget_code() != null){
												%>
												<tr>
													<td><%=k %></td>
													<% 
														if(item2.getUpdated_cost_center() == null){
															%>
																<td></td>
																<td></td>
															<%
														}else{
															%>
																<td><%=item2.getUpdated_cost_center() %></td>
																<td><%=item2.getCost_center() %></td>
															<%
														}
													%>
													<% 
														if(item2.getUpdated_asset_nm() == null){
															%>
																<td></td>
																<td></td>
															<%
														}else{
															%>
																<td><%=item2.getUpdated_asset_nm() %></td>
																<td><%=item2.getAsset_nm() %></td>
															<%
														}
													%>
													<% 
														if(item2.getUpdated_class_cd() == null){
															%>
																<td></td>
																<td></td>
															<%
														}else{
															%>
																<td><%=item2.getUpdated_class_cd() %></td>
																<td><%=item2.getClass_cd() %></td>
															<%
														}
													%>
													<% 
														if(item2.getUpdated_capital_budget_code() == null){
															%>
																<td></td>
																<td></td>
															<%
														}else{
															%>
																<td><%=item2.getUpdated_acq_val_comp() %></td>
																<td><%=item2.getAcq_val_comp() %></td>
															<%
														}
													%>
													<%
													if(item.getPicmea() != null){
														String userAndate = TotalModel.getUserInfo(item.getPicmea());
														String dateStr = userAndate.split("&")[1];
														%>
															<td><%=userAndate.split("&")[0] %></td>
														<%
													}else{
														%>
														<td></td>
														<%
													}
													if(item.getMgrmea() != null){
														String userAndate = TotalModel.getUserInfo(item.getMgrmea());
														String dateStr = userAndate.split("&")[1];
														%>
															<td><%=userAndate.split("&")[0] %></td>
														<%
													}else{
														%>
														<td></td>
														<%
													}
													if(item.getUpdated_date() != null){
														%>
															<td><%=item.getUpdated_date() %></td>
														<%
													}else{
														%>
														<td></td>
														<%
													}
													%>
												</tr>
												<%
												k = k+1;
											}
										
									}
								}
							%>
						</tbody>
					</table>
				</div>
			</div>	
		</div>
	</div>

</div>
			<%}
}%> 
