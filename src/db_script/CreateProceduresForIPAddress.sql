-- Convert IP Address To BigInt
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[ConvertIPToBigInt]')
					   AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
DROP FUNCTION [dbo].[ConvertIPToBigInt]
GO

CREATE FUNCTION [dbo].[ConvertIPToBigInt]  (@ip nvarchar(15))
RETURNS bigint
AS
BEGIN   
	DECLARE @startIndex smallint
	DECLARE @endIndex smallint
	DECLARE @final nvarchar(12)
	DECLARE @temp nvarchar(5)
	DECLARE @i smallint

	SET @startIndex = 0
	SET @final = ''
	SET @i = 0
	WHILE @i < 4
	BEGIN
		SET @endIndex = CHARINDEX('.', @ip, @startIndex)

		IF @endIndex = 0
		BEGIN
			SET @endIndex = LEN(@ip) + 1
		END
			
		SET @temp = RIGHT('00' + SUBSTRING(@ip, @startIndex, @endIndex - @startIndex), 3)
		SET @final = @final + @temp

		SET @startIndex = @endIndex + 1
		SET @i = @i + 1
	END

   RETURN CAST(@final as bigint)
END