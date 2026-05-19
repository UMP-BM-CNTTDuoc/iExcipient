$connString = "Data Source=103.140.249.152;Initial Catalog=iExcipient;User ID=iExcipientUser;Password=StrongPassword1!;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT TOP 20 IDThanhphan, Ten_INN, CAS_No, CauTrucPhanTu FROM d_Thanhphan"
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
    $dataset = New-Object System.Data.DataSet
    $adapter.Fill($dataset) | Out-Null
    $dataset.Tables[0] | Format-List
} catch {
    Write-Error $_.Exception.Message
} finally {
    $connection.Close()
}
