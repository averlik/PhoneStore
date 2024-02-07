/*namespace PhoneStore.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Уникальный идентификатор элемента корзины

        public int PhoneId { get; set; } // Идентификатор продукта 

        public string PhoneName { get; set; } // Наименование продукта 

        public int Quantity { get; set; } // Количество товара в корзине

        public double Price { get; set; } // Цена за единицу товара

        public double Total { get { return Quantity * Price; } } // Общая стоимость данного элемента корзины

        public string Image { get; set; } // Ссылка на изображение товара

        public CartItem()
        {
           
        }

        public CartItem(Phone phone)
        {
            // Конструктор при добавлении товара в корзину
            PhoneId = phone.Id;
            PhoneName = phone.Name;
            Price = phone.Price;
            Quantity = 1;
            Image = phone.Image;
           
        }
    }
}
*/
namespace PhoneStore.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Уникальный идентификатор элемента корзины

        public int PhoneId { get; set; } // Идентификатор продукта 

        public string PhoneName { get; set; } // Наименование продукта 

        public int Quantity { get; set; } // Количество товара в корзине

        public double Price { get; set; } // Цена за единицу товара

        public double Total { get { return Quantity * Price; } } // Общая стоимость данного элемента корзины

        public string Image { get; set; } // Ссылка на изображение товара

        public string UserId { get; set; } // Идентификатор пользователя

        public CartItem()
        {

        }

        public CartItem(Phone phone, string userId)
        {
            PhoneId = phone.Id;
            PhoneName = phone.Name;
            Price = phone.Price;
            Quantity = 1;
            Image = phone.Image;
            UserId = userId;
        }
    }
}
