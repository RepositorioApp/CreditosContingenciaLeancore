class CreditosForm {
    constructor() {
        this.initEvents();
        this.cargarModalidades();
        //this.cargarTiendas();
        this.cargarMarcas();
    }

    initEvents() {

        document.getElementById('btnConsultar').addEventListener('click', () => {
            let isValid = true;
            const requiredFields = ["txtNroDocumento"];
            requiredFields.forEach(id => {
                const input = document.getElementById(id);
                if (!input || !input.value.trim()) {
                    input.classList.add("is-invalid");
                    isValid = false;
                } else {
                    input.classList.remove("is-invalid");
                }
            });

            if (!isValid) {
                this.mostrarAlerta("Por favor completa todos los campos obligatorios.", "warning");

                //alert("Por favor completa todos los campos obligatorios.");
                return;
            }
            this.consultarCliente();
        });
        document.getElementById('optModalidad').addEventListener('change', () => this.cargarCuotas());
        document.getElementById('optMarca').addEventListener('change', () => this.cargarTiendas());


        document.getElementById('btnGuardar').addEventListener('click', () => {
            let isValid = true;


            const requiredFields = ["txtNroDocumento", "optMarca", "optTienda", "txtNumeroObligacion", "txtValorCredito", ,
                "txtInteresTeorico", "txtIvaInteres", "txtPjTasaPeriodo", "txtPsTasaEA", "optModalidad", "optNumCuotas"];

            requiredFields.forEach(id => {
                const input = document.getElementById(id);
                if (!input || !input.value.trim()) {
                    input.classList.add("is-invalid");
                    isValid = false;
                } else {
                    input.classList.remove("is-invalid");
                }
            });

            if (!isValid) {
                this.mostrarAlerta("Por favor completa todos los campos obligatorios.", "warning");

                //alert("Por favor completa todos los campos obligatorios.");
                return;
            }
            this.guardarCredito();
        });
        document.getElementById('btnCancelar').addEventListener('click', () => this.limpiarFormulario());
    }

    async limpiarFormulario() {
        document.getElementById("formularioCredito").reset();
        const select = document.getElementById('optNumCuotas');
        select.innerHTML = '<option value="">Seleccione</option>';
        const selectMarca = document.getElementById('optMarca');
        selectMarca.innerHTML = '<option value="">Seleccione</option>';



        document.getElementById('formularioCredito').style.display = 'none';
        document.getElementById('txtNombre').style.display = 'none';
        document.getElementById('lblNombre').style.display = 'none';
    }

    async consultarCliente() {
        const nro = document.getElementById('txtNroDocumento').value.trim();

        if (!nro) {
            this.mostrarAlerta("Ingrese un número de documento.", "warning");
            return;
        }

        try {
            const response = await fetch(`${apiBaseUrl}/api/creditos/consultarCliente?nro=${encodeURIComponent(nro)}`);
            if (!response.ok) throw new Error("Error HTTP");

            const data = await response.json();
            if (data.exito)
                if (data.resultado <= 0) {
                    await this.registrarNoCreado(nro);
                }

            await this.limpiarFormulario();


            const nombre = data.resultado?.[0]?.nombre || '';
            document.getElementById('txtNombre').value = nombre;
            document.getElementById('txtNroDocumento').value = nro;
            document.getElementById('txtNombre').style.display = 'block';
            document.getElementById('lblNombre').style.display = 'block';
            document.getElementById('formularioCredito').style.display = 'block';
            await this.cargarModalidades();
            //await this.cargarTiendas();
            await this.cargarMarcas();


        } catch (error) {
            this.mostrarAlerta("Error al consultar el cliente: " + error, "danger");

        }
    }

    async registrarNoCreado(nro) {
        try {
            await fetch(`${apiBaseUrl}/api/creditos/registrarNoCreado`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ nroDocumento: nro })
            });
        } catch (error) {
            this.mostrarAlerta("Error al registrar el cliente no creado. modalidades: " + error, "danger");
        }
    }

    async cargarModalidades() {
        try {
            const res = await fetch(`${apiBaseUrl}/api/creditos/modalidades`);
            if (!res.ok) throw new Error("Error HTTP");

            const data = await res.json();
            if (!data.exito) throw new Error(data.mensaje);
            const modalidades = data.resultado;

            const select = document.getElementById('optModalidad');
            select.innerHTML = '<option value="">Seleccione...</option>';
            modalidades.forEach(m => {
                const option = document.createElement('option');
                option.value = m.idPago;
                option.textContent = m.nombrePago;
                select.appendChild(option);
            });
        } catch (error) {
            this.mostrarAlerta("Error cargando modalidades: " + error.message, "danger");
        }
    }

    async cargarCuotas() {
        const modalidad = document.getElementById('optModalidad').value;

        if (!modalidad) {
            const select = document.getElementById('optNumCuotas');
            select.innerHTML = '<option value="">Seleccione</option>';
            return;
        }

        try {
            var response = await fetch(`${apiBaseUrl}/api/creditos/cuotas?modalidad=${encodeURIComponent(modalidad)}`);
            if (!response.ok) throw new Error("No se pudo cargar las cuotas");

            var cuotas = await response.json();

            var cta = cuotas.resultado;


            var select = document.getElementById('optNumCuotas');
            select.innerHTML = '<option value="">Seleccione</option>';

            cta.forEach(c => {
                const option = document.createElement('option');
                option.value = c.idPlazo;
                option.textContent = c.tiempoPlazo;
                select.appendChild(option);
            });
        } catch (error) {
            this.mostrarAlerta("Error al cargar cuotas: " + error, "danger");
        }
    }
    async cargarTiendas() {
        const marca = document.getElementById('optMarca').value;

        try {
            const res = await fetch(`${apiBaseUrl}/api/creditos/tiendas?marca=${encodeURIComponent(marca)}`);
            if (!res.ok) throw new Error("Error HTTP");

            const data = await res.json();
            if (!data.exito) throw new Error(data.mensaje);

            const tiendas = data.resultado;

            const select = document.getElementById('optTienda');
            select.innerHTML = '<option value="">Seleccione...</option>';
            tiendas.forEach(t => {
                const option = document.createElement('option');
                option.value = t.ceCo;
                option.textContent = t.tienda;
                select.appendChild(option);
            });
        } catch (error) {
            this.mostrarAlerta("Error al cargar las tiendas: " + error, "danger");
        }
    }

    async cargarMarcas() {
        try {
            const res = await fetch(`${apiBaseUrl}/api/creditos/marcas`);
            if (!res.ok) throw new Error("Error HTTP");

            const data = await res.json();
            if (!data.exito) throw new Error(data.mensaje);

            const marcas = data.resultado;

            const select = document.getElementById('optMarca');
            select.innerHTML = '<option value="">Seleccione...</option>';
            marcas.forEach(m => {
                const option = document.createElement('option');
                option.value = m.nombreMarca;
                option.textContent = m.descripcionMarca;
                select.appendChild(option);
            });
        } catch (error) {
            this.mostrarAlerta("Error al cargar las marcas: " + error, "danger");
        }
    }

    async guardarCredito() {
        const form = document.getElementById("formularioCredito");
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const selectModalidad = document.getElementById("optModalidad");
        const selectedOptionModalidad = selectModalidad.options[selectModalidad.selectedIndex];
        const modalidad = selectedOptionModalidad.text;
        const selectCuotas = document.getElementById("optNumCuotas");
        const selectedOptionCuota = selectCuotas.options[selectCuotas.selectedIndex];
        const cuotas = selectedOptionCuota.text;
        const selectTienda = document.getElementById("optTienda");
        const selectedTienda = selectTienda.options[selectTienda.selectedIndex];
        const tienda = selectedTienda.text;
        //const selectMarca = document.getElementById("optMarca");
        //const selectedMarca = selectMarca.options[selectMarca.selectedIndex];
        //const marca = selectedMarca.text;


        const dto = {
            nroDocumento: document.getElementById('txtNroDocumento').value.trim(),
            marca: document.getElementById('optMarca').value,
            tienda: tienda,
            obligacion: document.getElementById('txtNumeroObligacion').value.trim(),
            caja: document.getElementById('txtCaja').value.trim(),
            cajero: document.getElementById('txtCajero').value.trim(),
            valorCredito: document.getElementById('txtValorCredito').value,
            valorCuotaInicial: document.getElementById('txtValorCuotaInicial').value.toString(),
            interesTeorico: document.getElementById('txtInteresTeorico').value.toString(),
            ivaInteres: document.getElementById('txtIvaInteres').value,
            pjTasaPeriodo: document.getElementById('txtPjTasaPeriodo').value,
            psTasaEA: document.getElementById('txtPsTasaEA').value,
            descModalidad: modalidad.toString(),
            numCuotas: parseInt(cuotas),
            fechaVencimiento: document.getElementById("txtFechaVencimiento").value
        };

        if (!dto.fechaVencimiento) {
            dto.fechaVencimiento = "1900-01-01T00:00:00";
        }
        try {
            const res = await fetch(`${apiBaseUrl}/api/creditos/guardar`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dto)
            });

            if (res.ok) {
                this.limpiarFormulario();
                document.getElementById('txtNroDocumento').value = '';
                this.mostrarAlerta("Crédito guardado correctamente.", "success");

            } else {
                const err = await res.text();
                this.mostrarAlerta("Error al guardar crédito: " + err, "danger");

            }
        } catch (error) {
            this.mostrarAlerta("Error al guardar crédito: " + error, "danger");
        }
    }

    mostrarAlerta(mensaje, tipo = "info", tiempo = 10000) {
        const alertContainer = document.getElementById('alertContainer');

        const alerta = document.createElement('div');
        alerta.className = `alert alert-${tipo} alert-dismissible fade show`;
        alerta.setAttribute("role", "alert");
        alerta.innerHTML = `
        ${mensaje}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

        alertContainer.appendChild(alerta);

        setTimeout(() => {
            alerta.classList.remove('show');
            alerta.classList.add('hide');
            setTimeout(() => alerta.remove(), 300);
        }, tiempo);
    }


}

document.addEventListener('DOMContentLoaded', () => new CreditosForm());

