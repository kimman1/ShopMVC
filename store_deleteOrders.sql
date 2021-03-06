USE [ShopMVC]
GO
/****** Object:  StoredProcedure [dbo].[deleteOrders]    Script Date: 06/03/2020 16:44:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[deleteOrders]
@idOrder int,
@outputresult int OUTPUT
as
begin
	declare @idCheck int
	set @idCheck = (select OrderID from Orders where not exists (select OrderID from OrdersDetail where Orders.OrderID = OrdersDetail.OrderID ) and Orders.OrderID = @idOrder )
	if(@idCheck <> '')
	begin 
	delete from Orders where OrderID = @idOrder
	set @outputresult = 1
	end
	else
	begin
	set @outputresult = 0
	end
end
