﻿using WebApplication1.Models;
using WebApplication1.Settings;
using System.Data.SQLite;

namespace WebApplication1.Services {
    public class RestaurantsServices {
        public static List<Restaurant> GetAll(WhereSQL whereSQL) {
            try {
                List<Restaurant> list = new List<Restaurant>();
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {RestaurantSQLTable.tableName} {whereSQL?.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Restaurant {
                                    ID = Convert.ToInt64(reader[$"{RestaurantSQLTable.id}"].ToString()),
                                    Name = reader[$"{RestaurantSQLTable.name}"].ToString()
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

        public static Restaurant? GetSingle(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {RestaurantSQLTable.tableName} {whereSQL.GetClausule()}",dbContext)) {
                        using (SQLiteDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                return new Restaurant {
                                    ID = Convert.ToInt64(reader[$"{RestaurantSQLTable.id}"].ToString()),
                                    Name = reader[$"{RestaurantSQLTable.name}"].ToString()
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
        public static Restaurant Create(Restaurant newItem) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {RestaurantSQLTable.tableName} ({RestaurantSQLTable.name}) VALUES (?)",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{RestaurantSQLTable.name}",newItem.Name));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) throw new Exception("Error en base de datos");
                        newItem.ID = dbContext.LastInsertRowId;
                        return newItem;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static Restaurant? Edit(WhereSQL whereSQL,Restaurant item) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"UPDATE {RestaurantSQLTable.tableName} SET {RestaurantSQLTable.name} = ? {whereSQL.GetClausule()}",dbContext)) {
                        command.Parameters.Add(new SQLiteParameter($"{RestaurantSQLTable.name}",item.Name));
                        int changes = command.ExecuteNonQuery();
                        if (changes <= 0) return null;
                        return item;
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        public static int Delete(WhereSQL whereSQL) {
            try {
                using (SQLiteConnection dbContext = DBContext.GetInstance()) {
                    using (SQLiteCommand command = new SQLiteCommand($"DELETE FROM {RestaurantSQLTable.tableName} {whereSQL.GetClausule()}",dbContext)) {
                        return command.ExecuteNonQuery();
                    }
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
