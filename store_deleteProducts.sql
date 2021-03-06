USE [ShopMVC]
GO
/****** Object:  StoredProcedure [dbo].[deleteProduct]    Script Date: 06/03/2020 16:44:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[deleteProduct]
@idProduct int,
@outputresult int OUTPUT
as
begin
	declare @idCheck int
	set @idCheck = (select ProductID from Product where not exists (select ProductID from OrdersDetail where Product.ProductID = OrdersDetail.ProductID ) and Product.ProductID = @idProduct )
	if(@idCheck <> '')
	begin 
	delete from Product where ProductID = @idProduct
	set @outputresult = 1
	end
	else
	begin
	set @outputresult = 0
	end
end
