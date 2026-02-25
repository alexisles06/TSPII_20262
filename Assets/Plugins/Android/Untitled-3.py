def sumar_numeros():
    """
    Pide al usuario una cantidad de números (n), luego solicita n números,
    y finalmente imprime la suma de ellos.
    """
    suma_total = 0.0

    # Pedir la cantidad de números a sumar con validación
    while True:
        try:
            cantidad_numeros = int(input("¿Cuántos números deseas sumar? "))
            if cantidad_numeros > 0:
                break
            print("Por favor, introduce un número entero positivo.")
        except ValueError:
            print("Entrada no válida. Por favor, introduce un número entero.")

    # Bucle para pedir cada número y sumarlo
    for i in range(cantidad_numeros):
        
        while True:
            try:
                # Usamos float() para permitir números con decimales
                numero = float(input(f"Introduce el número {i + 1}: "))
                suma_total += numero
                break # Salir del bucle si la entrada es un número válido
            except ValueError:
                print("Entrada no válida. Por favor, introduce un número.")

    # Imprimir el resultado final
    print(f"\nLa suma de los {cantidad_numeros} números es: {suma_total}")

# Ejecutar la función principal si el script se corre directamente
if __name__ == "__main__":
    sumar_numeros()

