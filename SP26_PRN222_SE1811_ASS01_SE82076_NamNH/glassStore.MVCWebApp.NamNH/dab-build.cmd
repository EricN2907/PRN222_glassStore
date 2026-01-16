@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "OrdersNamNh" --source "[dbo].[Orders_NamNH]" --fields.include "order_id,user_id,voucher_id,order_type,order_code,payment_method,subtotal,discount_total,tax_total,shipping_fee,grand_total,status,customer_note,receiver_name,receiver_phone,receiver_address,created_at,updated_at" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo Adding relationships
@echo Adding stored procedures
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
