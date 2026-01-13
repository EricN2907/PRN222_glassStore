USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'glass_Store')
BEGIN
    CREATE DATABASE glass_Store;
END
GO

USE glass_Store;
GO

-- =============================================
-- TẠO BẢNG MỚI (UPDATE FULL FIELDS)
-- =============================================
-- 1. User & Role
CREATE TABLE [Role] (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL
);

CREATE TABLE [User] (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    role_id INT,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    full_name NVARCHAR(100),
    phone_number VARCHAR(20),
    status BIT DEFAULT 1,
    address NVARCHAR(255),        -- [PDF]
    date_of_birth DATETIME,       -- [PDF]
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (role_id) REFERENCES [Role](role_id)
);

-- 2. Danh mục & Brand
CREATE TABLE Brands_TanTM (
    brand_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    country NVARCHAR(50),
    slug VARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

CREATE TABLE Categories_TanTM (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX),
    parent_id INT,
    is_active BIT DEFAULT 1,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (parent_id) REFERENCES Categories_TanTM(category_id)
);

CREATE TABLE Vouchers_TanTM (
    voucher_id INT IDENTITY(1,1) PRIMARY KEY,
    code VARCHAR(50) NOT NULL UNIQUE,
    description NVARCHAR(255),
    discount_amount DECIMAL(18, 2),
    discount_percent INT,
    min_order_value DECIMAL(18, 2),
    start_date DATETIME,
    end_date DATETIME,
    usage_limit INT,
    is_active BIT DEFAULT 1
);

-- 3. Sản phẩm & Biến thể
CREATE TABLE Products_TanTM (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    category_id INT,
    brand_id INT,
    name NVARCHAR(200) NOT NULL,
    product_type NVARCHAR(50),
    product_code VARCHAR(50),
    short_description NVARCHAR(500),
    description NVARCHAR(MAX),
    base_price DECIMAL(18,2),     -- [PDF] Giá cơ bản
    is_active BIT DEFAULT 1,
    is_featured BIT DEFAULT 0,    -- [PDF] Sản phẩm nổi bật
    rating_avg DECIMAL(3, 2) DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (category_id) REFERENCES Categories_TanTM(category_id),
    FOREIGN KEY (brand_id) REFERENCES Brands_TanTM(brand_id)
);

CREATE TABLE Product_Variants_VanNB (
    variant_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT,
    color NVARCHAR(50),
    material NVARCHAR(50),
    size_label VARCHAR(20),
    sku VARCHAR(50) UNIQUE,
    gender NVARCHAR(20), -- [PDF] gender_fit
    unit_price DECIMAL(18, 2),
    weight_grams INT,             -- [PDF] Trọng lượng để tính ship
    is_preorder BIT DEFAULT 0,    -- [PDF] Có phải hàng đặt trước không
    preorder_eta_date DATETIME,   -- [PDF] Ngày dự kiến có hàng
    sold_count INT DEFAULT 0,
    predictor_eye_data NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (product_id) REFERENCES Products_TanTM(product_id)
);

CREATE TABLE Product_Media_VanNB (
    media_id INT IDENTITY(1,1) PRIMARY KEY,
    variant_id INT,
    product_id INT,               -- [PDF] Có link cả product
    media_url VARCHAR(500),
    media_type VARCHAR(20),
    sort_order INT,
    FOREIGN KEY (variant_id) REFERENCES Product_Variants_VanNB(variant_id),
    FOREIGN KEY (product_id) REFERENCES Products_TanTM(product_id)
);

CREATE TABLE Inventory_VanNB (
    inventory_id INT IDENTITY(1,1) PRIMARY KEY,
    variant_id INT,
    quantity_on_hand INT DEFAULT 0, -- [PDF] on_hand
    reserved INT DEFAULT 0,         -- [PDF] Hàng đang được giữ trong đơn chưa thanh toán
    reorder_level INT,
    warehouse_location NVARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (variant_id) REFERENCES Product_Variants_VanNB(variant_id)
);

CREATE TABLE Product_Reviews_VanNB (
    review_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    product_id INT,
    rating INT CHECK (rating >= 1 AND rating <= 5),
    comment NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES [User](user_id),
    FOREIGN KEY (product_id) REFERENCES Products_TanTM(product_id)
);

-- 4. Giỏ hàng
CREATE TABLE Cart_MinhLK (
    cart_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    status VARCHAR(20),           -- [PDF] Active/Abandoned
    note NVARCHAR(255),
    expires_at DATETIME,          -- [PDF] Thời gian hết hạn giỏ
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

CREATE TABLE Cart_Item_MinhLK (
    cart_item_id INT IDENTITY(1,1) PRIMARY KEY,
    cart_id INT,
    variant_id INT,
    quantity INT,
    unit_price_snapshot DECIMAL(18, 2),
    added_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (cart_id) REFERENCES Cart_MinhLK(cart_id),
    FOREIGN KEY (variant_id) REFERENCES Product_Variants_VanNB(variant_id)
);

-- 5. Đơn hàng
CREATE TABLE Orders_NamNH (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    voucher_id INT NULL,
    order_type VARCHAR(20),
    order_code VARCHAR(50) UNIQUE,
    payment_method VARCHAR(50),
    
    -- [PDF] Nhóm Price
    subtotal DECIMAL(18, 2),        -- Tổng tiền hàng
    discount_total DECIMAL(18, 2),  -- Tổng giảm giá
    tax_total DECIMAL(18, 2),		   -- Thuế
    shipping_fee DECIMAL(18, 2),    -- Phí ship
    grand_total DECIMAL(18, 2),     -- Tổng thanh toán cuối cùng (total_amount)
    
    status VARCHAR(50),
    customer_note NVARCHAR(MAX),    -- [PDF] Ghi chú khách hàng
    receiver_name NVARCHAR(100),
    receiver_phone VARCHAR(20),
    receiver_address NVARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (user_id) REFERENCES [User](user_id),
    FOREIGN KEY (voucher_id) REFERENCES Vouchers_TanTM(voucher_id)
);

CREATE TABLE Order_Detail_NamNH (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    variant_id INT,
    quantity INT,
    
    -- [PDF] Snapshot dữ liệu (Quan trọng)
    product_name_snapshot NVARCHAR(200), -- Tên SP lúc mua
    variant_desc_snapshot NVARCHAR(200), -- Màu/Size lúc mua
    unit_price_at_purchase DECIMAL(18, 2),
    line_total DECIMAL(18, 2),           -- quantity * price
    
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (order_id) REFERENCES Orders_NamNH(order_id),
    FOREIGN KEY (variant_id) REFERENCES Product_Variants_VanNB(variant_id)
);

-- 6. Thanh toán & Ship
CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    payment_method VARCHAR(50),
    amount DECIMAL(18, 2),
    transaction_code VARCHAR(100),
    gateway_transaction_id VARCHAR(100), -- [PDF]
    bank_code VARCHAR(20),
    status VARCHAR(20),
    paid_at DATETIME,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (order_id) REFERENCES Orders_NamNH(order_id)
);

CREATE TABLE Shipping_NhatM (
    shipping_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    shipping_method NVARCHAR(100),
    shipping_fee DECIMAL(18, 2),
    status VARCHAR(50),
    tracking_number VARCHAR(100),
    carrier NVARCHAR(100),
    shipping_note NVARCHAR(MAX),  -- [PDF]
    delivered_at DATETIME,        -- [PDF]
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (order_id) REFERENCES Orders_NamNH(order_id)
);

CREATE TABLE Tracking_Event_NhatM (
    tracking_event_id INT IDENTITY(1,1) PRIMARY KEY,
    shipping_id INT,
    event_time DATETIME,
    status VARCHAR(50),
    location NVARCHAR(200),
    note NVARCHAR(MAX),
    raw_data NVARCHAR(MAX),
    FOREIGN KEY (shipping_id) REFERENCES Shipping_NhatM(shipping_id)
);

-- 7. Toa thuốc (Chi tiết chuẩn y khoa)
CREATE TABLE Prescription_TanBN (
    prescription_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    verified_by_staff_id INT,
    verify_status VARCHAR(20),
    pd_mm DECIMAL(4, 2),           -- [PDF] Khoảng cách đồng tử
    image_url VARCHAR(500),
    note NVARCHAR(MAX),
    started_at DATETIME,           -- [PDF]
    completed_at DATETIME,         -- [PDF]
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES [User](user_id),
    FOREIGN KEY (verified_by_staff_id) REFERENCES [User](user_id)
);

CREATE TABLE Prescription_Eye_Details_TanBN (
    detail_id INT IDENTITY(1,1) PRIMARY KEY,
    prescription_id INT,
    eye_side VARCHAR(2) NOT NULL, -- OD/OS
    sph DECIMAL(4, 2),
    cyl DECIMAL(4, 2),
    axis INT,
    prism DECIMAL(4, 2),          -- [PDF] Độ lăng kính
    base_direction VARCHAR(20),   -- [PDF] Hướng đáy (Up/Down/In/Out)
    add_power DECIMAL(4, 2),
    note NVARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (prescription_id) REFERENCES Prescription_TanBN(prescription_id)
);

-- 8. Return & Preorder
CREATE TABLE Return_Request (
    return_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    user_id INT,
    status VARCHAR(50),
    reason_code VARCHAR(50),      -- [PDF]
    description NVARCHAR(MAX),
    requested_at DATETIME,        -- [PDF]
    approved_by_staff_id INT,
    refunded_at DATETIME,
    closed_at DATETIME,
    FOREIGN KEY (order_id) REFERENCES Orders_NamNH(order_id),
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

CREATE TABLE Preorder (
    preorder_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT,
    deposit_amount DECIMAL(18, 2),
    expected_arrival_date DATETIME,
    status VARCHAR(50),
    note NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (order_id) REFERENCES Orders_NamNH(order_id)
);

CREATE TABLE Preorder_Event (
    event_id INT IDENTITY(1,1) PRIMARY KEY,
    preorder_id INT,
    event_type VARCHAR(50),
    note NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (preorder_id) REFERENCES Preorder(preorder_id)
);


USE glass_Store;
GO
/****** Object:  Table [dbo].[System.UserAccount]    Script Date: 04/15/25 12:23:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[System.UserAccount](
	[UserAccountID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[EmployeeCode] [nvarchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[RequestCode] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ApplicationCode] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_System.UserAccount] PRIMARY KEY CLUSTERED 
(
	[UserAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[System.UserAccount] ON 
GO
INSERT [dbo].[System.UserAccount] ([UserAccountID], [UserName], [Password], [FullName], [Email], [Phone], [EmployeeCode], [RoleId], [RequestCode], [CreatedDate], [ApplicationCode], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'acc', N'@a', N'Accountant', N'Accountant@', N'0913652742', N'000001', 2, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[System.UserAccount] ([UserAccountID], [UserName], [Password], [FullName], [Email], [Phone], [EmployeeCode], [RoleId], [RequestCode], [CreatedDate], [ApplicationCode], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'auditor', N'@a', N'Internal Auditor', N'InternalAuditor@', N'0972224568', N'000002', 3, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[System.UserAccount] ([UserAccountID], [UserName], [Password], [FullName], [Email], [Phone], [EmployeeCode], [RoleId], [RequestCode], [CreatedDate], [ApplicationCode], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'chiefacc', N'@a', N'Chief Accountant', N'ChiefAccountant@', N'0902927373', N'000003', 1, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[System.UserAccount] OFF
GO
