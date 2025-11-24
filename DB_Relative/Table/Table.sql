CREATE TABLE api_receiveTN (
	ID varchar(33) NOT NULL,
    [controller] varchar(33) NULL,
    [data_api] xml NULL,
    [datetime0] datetime DEFAULT GETDATE(),
	  get_data_yn int DEFAULT  0,
    ghi_chu nvarchar(526) NULL, 
)
GO