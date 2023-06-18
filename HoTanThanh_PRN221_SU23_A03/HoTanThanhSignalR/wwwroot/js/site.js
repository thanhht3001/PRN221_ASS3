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
            url: '/Customers/Index?handler=Customers',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.email} </td>
                        <td> ${v.customerName} </td>
                        <td> ${v.city} </td>
                        <td> ${v.country} </td>
                        <td> ${v.password} </td>
                        <td> ${v.birthday} </td>
                        <td>
                <a href='/Customers/Edit?id=${v.customerId}'>Edit</a> |
                <a href='/Customers/Details?id=${v.customerId}'>Details</a> |
                <a href='/Customers/Delete?id=${v.customerId}'>Delete</a>
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
            url: '/FlowerBouquets/Index?handler=Flowers',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td> ${v.flowerBouquetName} </td>
                        <td> ${v.description} </td>
                        <td> ${v.unitPrice} </td>
                        <td> ${v.unitsInStock} </td>
                        <td> ${v.flowerBouquetStatus} </td>
                        <td> ${v.category.categoryName} </td>
                        <td> ${v.supplier.supplierName} </td>
                        <td>
                <a href='/FlowerBouquets/Edit?id=${v.flowerBouquetId}'>Edit</a> |
                <a href='/FlowerBouquets/Details?id=${v.flowerBouquetId}'>Details</a> |
                <a href='/FlowerBouquets/Delete?id=${v.flowerBouquetId}'>Delete</a>
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
