--MyMIS创建数据对象脚本
GO
IF EXISTS ( SELECT  1
            FROM    SYSOBJECTS
            WHERE   ID = OBJECT_ID('fn_Split')
                    AND XTYPE = 'TF' ) 
    BEGIN
        DROP FUNCTION dbo.fn_Split
    END
GO
--分隔字符串
CREATE FUNCTION [dbo].[fn_Split]
    (
      @String NVARCHAR(MAX) ,
      @Delimiter NVARCHAR(10)
    )
RETURNS @ValueTable TABLE
    (
      [id] INT ,
      [Value] NVARCHAR(MAX)
    )
    BEGIN      
        DECLARE @NextString NVARCHAR(MAX) ,
            @Pos INT ,
            @NextPos INT ,
            @CommaCheck NVARCHAR(1) ,
            @id INT    
    
        SET @id = 1      
       
        SET @NextString = ''      
        SET @CommaCheck = RIGHT(@String, 1)       
       
        SET @String = @String + @Delimiter      
       
        SET @Pos = CHARINDEX(@Delimiter, @String)      
        SET @NextPos = 1      
       
        WHILE ( @pos <> 0 ) 
            BEGIN      
                SET @NextString = SUBSTRING(@String, 1, @Pos - 1)      
        
                INSERT  INTO @ValueTable
                        ( [Value], [id] )
                VALUES  ( @NextString, @id )      
        
                SET @String = SUBSTRING(@String, @pos + 1, LEN(@String))      
         
                SET @NextPos = @Pos      
                SET @pos = CHARINDEX(@Delimiter, @String)      
    
                SET @id = @id + 1    
            END      
       
        RETURN      
    END 
GO





