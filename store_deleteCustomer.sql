USE [ShopMVC]
GO
/****** Object:  StoredProcedure [dbo].[deleteCustomer]    Script Date: 06/03/2020 16:44:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[deleteCustomer]
@idCustomer int,
@outputresult int OUTPUT
as
begin
	declare @idCheck int
	set @idCheck = (select CustomerID from Customer where not exists (select CustomerID from Orders where Orders.CustomerID = Customer.CustomerID ) and Customer.CustomerID = @idCustomer )
	if(@idCheck <> '')
	begin 
	delete from Customer where CustomerID = @idCustomer
	set @outputresult = 1
	end
	else
	begin
	set @outputresult = 0
	end
end
