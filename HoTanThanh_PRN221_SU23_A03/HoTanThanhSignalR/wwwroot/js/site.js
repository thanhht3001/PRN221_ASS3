// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(() => {
    LoadAppUserData();
    LoadFlowers();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();

    connection.on("LoadAppUsers", function () {
        LoadAppUserData();
    })
    connection.on("LoadFlowers", function () {
        LoadFlowers();
    })

    LoadAppUserData();
    LoadFlowers();

    function LoadAppUserData() {
        var tr = '';
        $.ajax({
            url: '/Customers',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.Email} </td>
                        <td> ${v.CustomerName} </td>
                        <td> ${v.City} </td>
                        <td> ${v.Country} </td>
                        <td> ${v.Password} </td>
                        <td> ${v.Birthday} </td>
                        <td>
                 <a asp-page="./Edit" asp-route-id="@item.CustomerId">Edit</a> |
                <a asp-page="./Details" asp-route-id="@item.CustomerId">Details</a> |
                <a asp-page="./Delete" asp-route-id="@item.CustomerId">Delete</a>
                </td> </tr>`
                })
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
    function LoadFlowers() {
        var tr = '';
        $.ajax({
            url: '/FlowerBouquets',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.FlowerBouquetName} </td>
                        <td> ${v.Description} </td>
                        <td> ${v.UnitPrice} </td>
                        <td> ${v.UnitsInStock} </td>
                        <td> ${v.FlowerBouquetStatus} </td>
                        <td> ${v.Category.CategoryName} </td>
                        <td> ${v.Supplier.SupplierName} </td>
                        <td>
                <a asp-page="./Edit" asp-route-id="@item.FlowerBouquetId">Edit</a> |
                <a asp-page="./Details" asp-route-id="@item.FlowerBouquetId">Details</a> |
                <a asp-page="./Delete" asp-route-id="@item.FlowerBouquetId">Delete</a>
                </td> </tr>`
                })
                $("#tableBody1").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})
