USE [ShopMVC]
GO
/****** Object:  StoredProcedure [dbo].[deleteCustomer]    Script Date: 06/03/2020 15:04:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc deleteCategory
@idCat int,
@outputresult int OUTPUT
as
begin
	declare @idCheck int
	set @idCheck = (select CategoryID from Category where not exists (select CategoryID from Product where Category.CategoryID = Product.ProductID ) and Category.CategoryID = @idCat )
	if(@idCheck <> '')
	begin 
	delete from Category where CategoryID = @idCat
	set @outputresult = 1
	end
	else
	begin
	set @outputresult = 0
	end
end
