<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@include file="/templates/inc/Subheader.jsp"%>
<%@include file="/templates/inc/leftBar.jsp"%>
<%@include file="/templates/inc/header.jsp"%>
<%@page import="org.o7planning.bean.TotalBean"%>
<%@page import="java.util.ArrayList"%>
<%@page import="org.o7planning.model.HomeModel" %>
<%@page import="java.time.LocalDate" %>
<!-- main code -->
<!-- Begin Page Content -->
<%
	ArrayList<String> lstI = (ArrayList<String>)(new HomeModel()).getListCard();
%>
<style>
	.container-fluid{
		
	}
</style>
<div class="container-fluid">
	<div class="d-sm-flex align-items-center justify-content-between mb-4">
		<div>
			<h1 class="h3 mb-0 text-gray-800">Home</h1>
		</div>
		<form class="d-flex align-items-center" action="<%=request.getContextPath()%>/Home">
			<select class="form-control form-control-sm mr-2" name="factory" id="">
				<option value="IJP">IJP - Thang Long & Tien Son</option>
				<option value="LBP">LBP - Que Vo</option>
			</select> <input type="submit"
				class="d-none d-sm-inline-block btn btn-sm btn-primary-custom shadow-sm"
				value = "Generate" />
		</form>
	</div>
	<div class="row">
		<div class="col-xl-3 col-md-6 mb-4">
			<div class="card border-left-primary shadow h-100 py-2">
				<div class="card-body">
					<div class="row no-gutters align-items-center">
						<div class="col mr-2">
							<div class="text-xs font-weight-bold text-primary text-uppercase mb-1">Total
								(Assets)</div>
							<div class="h5 mb-0 font-weight-bold text-gray-800"><%=lstI.get(0) %></div>
						</div>
						<div class="col-auto">
							<i class="fas fa-calculator fa-2x text-gray-300"></i>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="col-xl-3 col-md-6 mb-4">
			<div class="card border-left-success shadow h-100 py-2">
				<div class="card-body">
					<div class="row no-gutters align-items-center">
						<div class="col mr-2">
							<div
								class="text-xs font-weight-bold text-success text-uppercase mb-1">LBP
								(Inside)</div>
							<div class="h5 mb-0 font-weight-bold text-gray-800"><%=lstI.get(1) %></div>
						</div>
						<div class="col-auto">
							<i class="fas fa-th-large fa-2x text-gray-300"></i>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="col-xl-3 col-md-6 mb-4">
			<div class="card border-left-info shadow h-100 py-2">
				<div class="card-body">
					<div class="row no-gutters align-items-center">
						<div class="col mr-2">
							<div
								class="text-xs font-weight-bold text-info text-uppercase mb-1">IJP
								(Inside)</div>
							<div class="row no-gutters align-items-center">
								<div class="col-auto">
									<div class="h5 mb-0 mr-3 font-weight-bold text-gray-800"><%=lstI.get(2) %></div>
								</div>
								<div class="col">
									<!-- <div class="progress progress-sm mr-2">
										<div class="progress-bar bg-info" role="progressbar"
											style="width: 100%" aria-valuenow="100" aria-valuemin="0"
											aria-valuemax="100"></div>
									</div> -->
								</div>
							</div>
						</div>
						<div class="col-auto">
							<i class="fas fa-clipboard-list fa-2x text-gray-300"></i>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="col-xl-3 col-md-6 mb-4">
			<div class="card border-left-warning shadow h-100 py-2">
				<div class="card-body">
					<div class="row no-gutters align-items-center">
						<div class="col mr-2">
							<div
								class="text-xs font-weight-bold text-warning text-uppercase mb-1">Outside
								</div>
							<div class="h5 mb-0 font-weight-bold text-gray-800"><%=lstI.get(3) %></div>
						</div>
						<div class="col-auto">
							<i class="fas fa-comments fa-2x text-gray-300"></i>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="row" style="width: 100%; margin: 5rem 0 !important;">
		<div class="chartjs-size-monitor">
			<div class="chartjs-size-monitor-expand">
				<div></div>
			</div>
			<div class="chartjs-size-monitor-shrink">
				<div></div>
			</div>
		</div>
		<canvas width="1055" height="527" class="chartjs-render-monitor"
			id="canvas" style="width: 1055px; height: 527px; display: block;"></canvas>
	</div>
	<div class="row" style="width: 100%; margin: 5rem 0 !important;">
		<div class="chartjs-size-monitor">
			<div class="chartjs-size-monitor-expand">
				<div></div>
			</div>
			<div class="chartjs-size-monitor-shrink">
				<div></div>
			</div>
		</div>
		<canvas width="1055" height="527" class="chartjs-render-monitor"
			id="canvas2" style="width: 1055px; height: 527px; display: block;"></canvas>
	</div>	
	<div class="row" style="width: 100%; margin: 5rem 0 !important;">
		<div class="chartjs-size-monitor">
			<div class="chartjs-size-monitor-expand">
				<div></div>
			</div>
			<div class="chartjs-size-monitor-shrink">
				<div></div>
			</div>
		</div>
		<canvas width="1055" height="527" class="chartjs-render-monitor"
			id="canvas3" style="width: 1055px; height: 527px; display: block;"></canvas>
	</div>	
</div>
<%
	ArrayList<String> listStack = (ArrayList<String>) request.getAttribute("bieuDoInsideOutSide");
	String monthTit = (String) request.getAttribute("monthtitle");
	String monthTit1 = monthTit.substring(0, monthTit.length());
	String dataTreatment = (String) request.getAttribute("bieuDoTreatment");
	String dataStackSimple1 = (String) request.getAttribute("bieuDoTheoLoai1");
	String kindChart = (String) request.getAttribute("kind");
	String fact = ((String) request.getAttribute("factory")).toUpperCase();
	LocalDate currentDate = LocalDate.now();
	int iYear = currentDate.getYear();
%>
<!-- end main code -->
<%@include file="/templates/inc/footer.jsp"%>
<script>
var barChartData = {
		labels : [['IJP   LBP', '<%=iYear - 2%>'],['IJP   LBP', '<%=iYear - 1%>'],['IJP   LBP', 'January'], ['IJP   LBP', 'February'], ['IJP   LBP', 'March'], 
			['IJP   LBP', 'April'], ['IJP   LBP', 'May'], ['IJP   LBP', 'June'], 
			['IJP   LBP', 'July'], ['IJP   LBP', 'August'], ['IJP   LBP', 'September'], 
			['IJP   LBP', 'October'], ['IJP   LBP', 'November'], ['IJP   LBP', 'December']],
		datasets : [
			{
				label : 'Inside',
				backgroundColor : '#ccffcc',
				stack: 'IJP',
				data : [ <%=listStack.get(0) %> ]
			},
			{
				label : 'Outside',
				backgroundColor : '#ccccff',
				stack: 'IJP',
				data : [ <%=listStack.get(1) %> ]
			},
			{
				label : 'Total',
				type: 'scatter',
				backgroundColor : '#000066',
				pointRadius: 0,
				stack: 'IJP',
				data : [ <%=listStack.get(2) %> ]
			},
			{
				label : 'Inside',
				backgroundColor : '#ccffcc',
				stack: 'LBP',
				data : [ <%=listStack.get(3) %> ]
			},
			{
				label : 'Outside',
				backgroundColor : '#ccccff',
				stack: 'LBP',
				data : [ <%=listStack.get(4) %> ]
			},
			{
				label : 'Total',
				type: 'scatter',
				backgroundColor : '#000066',
				pointRadius: 0,
				stack: 'LBP',
				data : [ <%=listStack.get(5) %> ]
			}
			]

};
// 	var barChartData = {
// 			labels : [['IJP   LBP', 'January'], ['IJP   LBP', 'February'], ['IJP   LBP', 'March'], 
// 				['IJP   LBP', 'April'], ['IJP   LBP', 'May'], ['IJP   LBP', 'June'], 
// 				['IJP   LBP', 'July'], ['IJP   LBP', 'August'], ['IJP   LBP', 'September'], 
// 				['IJP   LBP', 'October'], ['IJP   LBP', 'November'], ['IJP   LBP', 'December']],
// 			datasets : [
// 				{
// 					label : 'Inside',
// 					backgroundColor : '#ccffcc',
// 					stack: 'IJP',
<%-- 					data : [ <%=listStack.get(0) %> ] --%>
// 				},
// 				{
// 					label : 'Outside',
// 					backgroundColor : '#ccccff',
// 					stack: 'IJP',
<%-- 					data : [ <%=listStack.get(1) %> ] --%>
// 				},
// 				{
// 					label : 'Total',
// 					type: 'scatter',
// 					backgroundColor : '#000066',
// 					pointRadius: 0,
// 					stack: 'IJP',
<%-- 					data : [ <%=listStack.get(2) %> ] --%>
// 				},
// 				{
// 					label : 'Inside',
// 					backgroundColor : '#ccffcc',
// 					stack: 'LBP',
<%-- 					data : [ <%=listStack.get(3) %> ] --%>
// 				},
// 				{
// 					label : 'Outside',
// 					backgroundColor : '#ccccff',
// 					stack: 'LBP',
<%-- 					data : [ <%=listStack.get(4) %> ] --%>
// 				},
// 				{
// 					label : 'Total',
// 					type: 'scatter',
// 					backgroundColor : '#000066',
// 					pointRadius: 0,
// 					stack: 'LBP',
<%-- 					data : [ <%=listStack.get(5) %> ] --%>
// 				}
// 				]

// 	};
var barChartData2 = {
		labels : [['IJP   LBP', '<%=iYear - 2%>'],['IJP   LBP', '<%=iYear - 1%>'],['IJP   LBP', 'January'], ['IJP   LBP', 'February'], ['IJP   LBP', 'March'], 
			['IJP   LBP', 'April'], ['IJP   LBP', 'May'], ['IJP   LBP', 'June'], 
			['IJP   LBP', 'July'], ['IJP   LBP', 'August'], ['IJP   LBP', 'September'], 
			['IJP   LBP', 'October'], ['IJP   LBP', 'November'], ['IJP   LBP', 'December']],
		datasets : [
			<%=dataStackSimple1%>
				 ]
};
// 	var barChartData2 = {
// 			labels : [['IJP   LBP', 'January'], ['IJP   LBP', 'February'], ['IJP   LBP', 'March'], 
// 				['IJP   LBP', 'April'], ['IJP   LBP', 'May'], ['IJP   LBP', 'June'], 
// 				['IJP   LBP', 'July'], ['IJP   LBP', 'August'], ['IJP   LBP', 'September'], 
// 				['IJP   LBP', 'October'], ['IJP   LBP', 'November'], ['IJP   LBP', 'December']],
// 			datasets : [
<%-- 				<%=dataStackSimple1%> --%>
// 					 ]
// 	};
	var barChartData3 = {
			labels : ['January', 'February', 'March', 
				'April', 'May', 'June', 
				'July', 'August', 'September', 
				'October', 'November', 'December'],
			datasets : [
				<%=dataTreatment%>
					 ]
	};
	function numberWithCommas(x) {
	    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
	}
	window.onload = function() {
		//=============================================================================================
		//bieu do theo in/out
		var ctx = document.getElementById('canvas').getContext('2d');
		window.myBar = new Chart(ctx, {
			type : 'bar',
			data : barChartData,
			options : {
				title : {
					display : true,
					text : 'MONTHLY QUANTITY  BY IN/OUT SIDE',
					fontSize : 20,
					fontStyle : 'bold'
				},
				animation: {
				      "duration": 1,
				      "onComplete": function() {
				        var chartInstance = this.chart,
				        ctx = chartInstance.ctx;
				        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
				        ctx.textAlign = 'center';
				        ctx.textBaseline = 'bottom';
				        
						var total = this.data.datasets.length - 1;
				        this.data.datasets.forEach(function(dataset, i) {				
				        	if(i == total || i == total - 3){
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(12.5, 'bold', Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = 'red';
						            if(data != 0 && i % 2 == 0){
						            	ctx.fillText(numberWithCommas(data), bar._model.x - 20, bar._model.y - 5);
						            }else if(data != 0 && i % 2 == 1){
						            	ctx.fillText(numberWithCommas(data), bar._model.x + 20, bar._model.y - 5);
						            }
						          });
				        	}else{
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = '#000066';
						            if(data != 0){
						            	ctx.fillText(numberWithCommas(data), bar._model.x, bar._model.y + 25);
						            }					            
						          });
				        	}
				        });
				      }
				},
				tooltips : {
 					mode : 'point',
 					intersect : true,
 					enabled: false
				},
				hover: {animationDuration: 1, mode: null},
				legend: {
					onClick: false,
					display: true,
					labels: {
		                filter: function (legendItem, chartData) {
		                	if(legendItem.datasetIndex < 2){
		                		return (chartData.datasets[legendItem.datasetIndex].label);
		                	}
		                }
		            }
				},
				responsive : true,
				scales : {
					xAxes : [ {
						stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Monthly <%=iYear%>'
						}

					} ],
					yAxes : [
						{
						//stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Quantity'
						}
					}]
				}
			}
		});
		//=============================================================================================
		//bieu do kind
		var ctx2 = document.getElementById('canvas2').getContext('2d');
		window.myBar = new Chart(ctx2, {
			type : 'bar',
			data : barChartData2,
			options : {
				title : {
					display : true,
					text : 'MONTHLY QUANTITY BY KINDS',
					fontSize : 20,
					fontStyle : 'bold'
				},
				animation: {
				      "duration": 1,
				      "onComplete": function() {
				        var chartInstance = this.chart,
				        ctx = chartInstance.ctx;
				        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
				        ctx.textAlign = 'center';
				        ctx.textBaseline = 'bottom';
				        
						var total = this.data.datasets.length - 1;
				        this.data.datasets.forEach(function(dataset, i) {				
				        	if(i == total || i == total - 1){
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(12.5, 'bold', Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = 'red';
						            if(data != 0 && i == 8){
						            	ctx.fillText(numberWithCommas(data), bar._model.x - 20, bar._model.y - 5);
						            }else if(data != 0 && i == 9){
						            	ctx.fillText(numberWithCommas(data), bar._model.x + 20, bar._model.y - 5);
						            }
						          });
				        	}else{
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = '#000066';
						            if(data != 0){
						            	ctx.fillText(numberWithCommas(data), bar._model.x, bar._model.y + 25);
						            }					            
						          });
				        	}
				        });
				      }
				},
				tooltips : {
					mode : 'point',
					intersect: true,
					enabled: false
				},
				hover: {animationDuration: 1, mode: null},
				legend: {
					onClick: false,
					display: true,
					labels: {
		                filter: function (legendItem, chartData) {
		                	if(legendItem.datasetIndex % 2 == 0 && legendItem.datasetIndex != 8 && legendItem.datasetIndex != 9){
		                		return (chartData.datasets[legendItem.datasetIndex].label);
		                	}
		                }
		            }
				},
				responsive : true,
				scales : {
					xAxes : [ {
						stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Monthly <%=iYear%>'
						}
					} ],
					yAxes : [ {
						//stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Kind'
						}
					} ]
				}
			}
		});
		//================================================================================================
		//bieu do treatment
		var ctx3 = document.getElementById('canvas3').getContext('2d');
		window.myBar = new Chart(ctx3, {
			type : 'bar',
			data : barChartData3,
			options : {
				title : {
					display : true,
					text : '<%= fact%> MONTHLY QUANTITY BY NEW FA AND TREATMENT',
					fontSize : 20,
					fontStyle : 'bold'
				},
				tooltips : {
					mode : 'index',
					intersect: true
				},
				animation: {
				      "duration": 1,
				      "onComplete": function() {
				        var chartInstance = this.chart,
				        ctx = chartInstance.ctx;
				        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
				        ctx.textAlign = 'center';
				        ctx.textBaseline = 'bottom';
				        
						//var total = this.data.datasets.length - 1;
				        this.data.datasets.forEach(function(dataset, i) {		
				        	if(i == 1){
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = '#000066';
						            if(data != 0){
						            	ctx.fillText(numberWithCommas(data), bar._model.x, bar._model.y + 25);
						            }					            
						          });
				        	}else if(i == 0){
				        		var meta = chartInstance.controller.getDatasetMeta(i);
						          meta.data.forEach(function(bar, index) {
						            var data = dataset.data[index];
						            ctx.font = Chart.helpers.fontString(15, 'bold', Chart.defaults.global.defaultFontFamily);
						            ctx.fillStyle = 'red';
						            if(data != 0){
						            	ctx.fillText(numberWithCommas(data), bar._model.x, bar._model.y - 25);
						            }					            
						          });
				        	}
				        });
				      }
				},
				responsive : true,
				scales : {
					xAxes : [ {
						stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Monthly <%=iYear%>'
						}
					} ],
					yAxes : [ {
						stacked : true,
						scaleLabel : {
							display : true,
							labelString : 'Treatment'
						}
					} ]
				}
			}
		});
	}
</script>

