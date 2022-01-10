﻿using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class FoodsServices {
        public static List<Food> GetAll() {
            try {
                List<Food> list = new List<Food>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Foods",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Food {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Restaurant = (reader["id_restaurant"] != null) ? RestaurantsServices.GetSingle(Convert.ToInt64(reader["id_restaurant"].ToString())) : null,
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Calories = Convert.ToDouble(reader["calories"])
                                });
                            }
                        }
                    }
                }
                return list;
            } catch (Exception) {
                throw;
            }
        }

        public static Food? GetSingle(long id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Foods WHERE id = " + id,dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Food {
                                    ID = Convert.ToInt64(reader["id"].ToString()),
                                    Restaurant = RestaurantsServices.GetSingle(Convert.ToInt64(reader["id_restaurant"].ToString())),
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    Calories = Convert.ToDouble(reader["calories"])
                                };
                            }
                        }
                    }
                }
            } catch (Exception) {
                throw;
            }
            return null;
        }
        public static Food? Create(Food newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO Foods (id_restaurant, name, description, calories) VALUES (?, ?, ?, ?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("id_restaurant",newItem.Restaurant?.ID));
                        command.Parameters.Add(new SQLiteParameter("name",newItem.Name));
                        command.Parameters.Add(new SQLiteParameter("description",newItem.Description));
                        command.Parameters.Add(new SQLiteParameter("calories",newItem.Calories));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) return null;
                        newItem.ID = dbContext.LastInsertRowId;
                        if(newItem.Restaurant?.ID != null) {
                            newItem.Restaurant = RestaurantsServices.GetSingle(newItem.Restaurant.ID);
                        }
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static Food? Edit(long id,Food item) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("UPDATE Foods SET name = ?, description = ?, calories = ? WHERE ID = " + id,dbContext)) {
                        command.Parameters.Add(new SQLiteParameter("name",item.Name));
                        command.Parameters.Add(new SQLiteParameter("description",item.Description));
                        command.Parameters.Add(new SQLiteParameter("calories",item.Calories));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) return null;
                        item.ID = id;
                        return item;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static int Delete(long id) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Foods WHERE id = " + id,dbContext)) {
                        return command.ExecuteNonQuery();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
