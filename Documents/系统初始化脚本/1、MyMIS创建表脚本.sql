--创建业务系统表
DROP TABLE [dbo].[AppFunction];
GO

/****** Object:  Table [dbo].[AppFunction]    Script Date: 01/19/2015 23:33:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AppFunction](
	[AppFunctionGUID] [uniqueidentifier] NOT NULL,
	[BusinessAppGUID] [uniqueidentifier] NULL,
	[FunctionName] [nvarchar](50) NULL,
	[FunctionCode] [nvarchar](200) NULL,
	[FunctionShortCode] [nvarchar](20) NULL,
	[IsDisabled] [tinyint] NULL,
	[DisabledReason] [nvarchar](400) NULL,
	[FunctionUrl] [nvarchar](400) NULL,
	[FunctionIcon] [nvarchar](100) NULL,
	[IconClass] [nvarchar](30) NULL,
	[IconClssUrl] [nvarchar](200) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[Remark] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AppFunctionGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[Authority];

GO

/****** Object:  Table [dbo].[Authority]    Script Date: 01/19/2015 23:33:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Authority](
	[AuthorityGUID] [uniqueidentifier] NOT NULL,
	[AuthorityCode] [nvarchar](50) NULL,
	[AuthorityName] [nvarchar](200) NULL,
	[AppFunctionGUID] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_Authority] PRIMARY KEY CLUSTERED 
(
	[AuthorityGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[BusinessApp];

GO

/****** Object:  Table [dbo].[BusinessApp]    Script Date: 01/19/2015 23:33:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BusinessApp](
	[BusinessAppGUID] [uniqueidentifier] NOT NULL,
	[BusinessAppName] [nvarchar](50) NULL,
	[BusinessAppCode] [nvarchar](200) NULL,
	[BusinessAppShortCode] [nvarchar](20) NULL,
	[ParentGUID] [uniqueidentifier] NULL,
	[IsDisabled] [tinyint] NULL,
	[DisabledReason] [nvarchar](400) NULL,
	[AppUrl] [nvarchar](400) NULL,
	[AppIcon] [nvarchar](100) NULL,
	[IconClass] [nvarchar](30) NULL,
	[IconClassUrl] [nvarchar](200) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[Remark] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[BusinessAppGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[Organization];

GO

/****** Object:  Table [dbo].[Organization]    Script Date: 01/19/2015 23:34:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Organization](
	[OrgGUID] [uniqueidentifier] NOT NULL,
	[OrgCode] [nchar](10) NULL,
	[OrgName] [nchar](10) NULL,
	[ParentGUID] [uniqueidentifier] NULL,
	[OrgType] [nvarchar](10) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[OrgGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[Role];

GO

/****** Object:  Table [dbo].[Role]    Script Date: 01/19/2015 23:34:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Role](
	[RoleGUID] [uniqueidentifier] NOT NULL,
	[RoleCode] [nvarchar](50) NULL,
	[RoleName] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[RoleAuth];

GO

/****** Object:  Table [dbo].[RoleAuth]    Script Date: 01/19/2015 23:34:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoleAuth](
	[RoleAuthGUID] [uniqueidentifier] NOT NULL,
	[RoleGUID] [uniqueidentifier] NULL,
	[AuthorityGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RoleAuth] PRIMARY KEY CLUSTERED 
(
	[RoleAuthGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[User];

GO

/****** Object:  Table [dbo].[User]    Script Date: 01/19/2015 23:34:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[UserGUID] [uniqueidentifier] NOT NULL,
	[UserCode] [nvarchar](50) NULL,
	[UserName] [nvarchar](200) NULL,
	[PassWord] [nvarchar](200) NULL,
	[OrgGUID] [uniqueidentifier] NULL,
	[PhotoUrl] [nvarchar](200) NULL,
	[OfficePhone] [nvarchar](20) NULL,
	[TelPhone] [nvarchar](20) NULL,
	[HomePhone] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[QQ] [nvarchar](20) NULL,
	[IsLocked] [tinyint] NULL,
	[LockedReason] [nvarchar](500) NULL,
	[IsDisabled] [tinyint] NULL,
	[DisabledReason] [nvarchar](500) NULL,
	[Gender] [tinyint] NULL,
	[Birthday] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[Remark] [nvarchar](500) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP TABLE [dbo].[UserRole];

GO

/****** Object:  Table [dbo].[UserRole]    Script Date: 01/19/2015 23:34:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRole](
	[UserRoleGUID] [uniqueidentifier] NOT NULL,
	[UserGUID] [uniqueidentifier] NULL,
	[RoleGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
