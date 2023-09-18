
$(document).ready(function () {
    $("#borrarPago").click(function (event) {
      $("#FechaPago").val(null);
    });
  });
  
$(document).ready(function () {
  $(".elSelect").select2();
});
  
  function init() {
    $(".alert").alert();
  };

  $(document).ready(function () {
    setTimeout(function () {
        $(".alert").alert('close');
    }, 5000); // 5000 milisegundos (5 segundos)
});
$(document).ready(function () {
  // Setup - add a text input to each footer cell
  $(".tableplus thead tr")
    .clone(true)
    .addClass("filters")
    .appendTo(".tableplus thead");

  var table = $(".tableplus").DataTable({
    language: {
      lengthMenu: "Mostrar _MENU_ registros por página",
      zeroRecords: "Nada que mostrar",
      info: "Página _PAGE_ de _PAGES_",
      infoEmpty: "No hay nada",
      infoFiltered: "(filtered from _MAX_ total records)",
      search: "Buscar:",
      paginate: {
        previous: "Pagina previa",
        next: "Pagina siguiente",
      },
    },
    orderCellsTop: true,
    fixedHeader: true,
    initComplete: function () {
      var api = this.api();

      // For each column
      api
        .columns()
        .eq(0)
        .each(function (colIdx) {
          // Set the header cell to contain the input element
          var cell = $(".filters th").eq(
            $(api.column(colIdx).header()).index()
          );
          var title = $(cell).text();
          $(cell).html('<input type="text" placeholder="" />');

          // On every keypress in this input
          $(
            "input",
            $(".filters th").eq($(api.column(colIdx).header()).index())
          )
            .off("keyup change")
            .on("change", function (e) {
              // Get the search value
              $(this).attr("title", $(this).val());
              var regexr = "({search})"; //$(this).parents('th').find('select').val();

              // Search the column for that value
              api
                .column(colIdx)
                .search(
                  this.value != ""
                    ? regexr.replace("{search}", "(((" + this.value + ")))")
                    : "",
                  this.value != "",
                  this.value == ""
                )
                .draw();
            })
            .on("keyup", function (e) {
              e.stopPropagation();

              if (e.key === "Enter") {
                // Detect Enter key press and trigger change event
                $(this).trigger("change");
              }
            });
        });
    },
  });
});


$(document).ready(function () {
  $(".table").DataTable({
    language: {
      lengthMenu: "Mostrar _MENU_ registros por página",
      zeroRecords: "Nada que mostrar",
      info: "Página _PAGE_ de _PAGES_",
      infoEmpty: "No hay nada",
      infoFiltered: "(filtered from _MAX_ total records)",
      search: "Buscar:",
      paginate: {
        previous: "Pagina previa",
        next: "Pagina siguiente",
      },
    },
  });
});