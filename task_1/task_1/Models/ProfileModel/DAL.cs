using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class DAL : IDisposable
    {
        private ProfileModelContext context = new ProfileModelContext();

        public ICollection<Profile> GetAllProfiles()
        {
            return context.Profile.ToList();
        }

        public void AddProfile(string username)
        {
            context.Profile.Add(new Profile() { UserName = username });
            context.SaveChanges();
        }

        public void DeleteProfile(int ID)
        {
            var profile = context.Profile.SingleOrDefault(x => x.Id == ID);
            if (profile != null)
            {
                foreach (var card in profile.BusinessCards)
                {
                    var fields = context.BusinessCardsToFields.Where(x => x.BusinessCardId == card.Id);
                    context.BusinessCardsToFields.RemoveRange(fields);
                }
                context.Profile.Remove(profile);
                context.SaveChanges();
            }
        }

        public Profile GetProfile(string username)
        {
            var profile = context.Profile.SingleOrDefault(x => x.UserName == username);
            return profile;
        }

        public Profile GetProfile(int ID)
        {
            return context.Profile.SingleOrDefault(x => x.Id == ID);
        }

        public void UpdateProfileFields(string username, ICollection<Field> fields)
        {
            var profile = context.Profile.SingleOrDefault(x => x.UserName == username);
            if (fields != null)
            {
                while (profile.Fields.Count != fields.Count)
                {
                    profile.Fields.Add(new Field());
                }
                for (int i = 0; i < fields.Count; i++)
                {
                    if (fields.ElementAt(i).Value == null)
                    {
                        context.Fields.Remove(profile.Fields.ElementAt(i));
                    }
                    else
                    {
                        profile.Fields.ElementAt(i).Type = fields.ElementAt(i).Type;
                        profile.Fields.ElementAt(i).Value = fields.ElementAt(i).Value;
                    }
                }
                context.SaveChanges();
            }
        }

        public BusinessCard GetBusinessCard(int cardID)
        {
            return context.BusinessCards.SingleOrDefault(x => x.Id == cardID);
        }

        public BusinessCard GetBusinessCardByUrl(string url)
        {
            return context.BusinessCards.SingleOrDefault(x => x.Url == url);
        }

        public void AddBusinessCardToUser(string username, BusinessCard card)
        {
            var profile = context.Profile.SingleOrDefault(x => x.UserName == username);
            profile.BusinessCards.Add(card);
            context.SaveChanges();
        }

        public void SetBusinessCardUrl(int ID, string url)
        {
            var businessCard = context.BusinessCards.SingleOrDefault(x => x.Id == ID);
            businessCard.Url = url;
            context.SaveChanges();
        }

        public void RemoveAllFieldsFromBusinessCard(int ID)
        {
            foreach(var line in context.BusinessCardsToFields)
            {
                if (line.BusinessCardId == ID)
                {
                    context.BusinessCardsToFields.Remove(line);
                }
            }
            context.SaveChanges();
        }

        public void AddFieldsToBusinessCard(string username, int cardID, String[] fields, String[] coordinates)
        {
            var profile = context.Profile.SingleOrDefault(x => x.UserName == username);
            for (int i = 0; i < fields.Length; i++)
            {
                context.BusinessCardsToFields.Add(
                    new BusinessCardToField()
                    {
                        FieldId = profile.Fields.SingleOrDefault(x => x.Value == fields[i]).Id,
                        BusinessCardId = cardID,
                        OffsetTop = Convert.ToInt32(coordinates[i * 2]),
                        OffsetLeft = Convert.ToInt32(coordinates[i * 2 + 1])
                    });
            }
            context.SaveChanges();

        }

        public void DeleteBusinessCard(int cardID)
        {
            if (context.BusinessCards.Any(x => x.Id == cardID))
            {
                context.BusinessCards.Remove(context.BusinessCards.SingleOrDefault(x => x.Id == cardID));
                var cardFields = from line in context.BusinessCardsToFields
                                 where line.BusinessCardId == cardID
                                 select line;
                foreach (var cf in cardFields)
                {
                    context.BusinessCardsToFields.Remove(cf);
                }
                context.SaveChanges();
            }
        }

        public void IncreaseCardRating(int cardID, int rate)
        {
            var card = context.BusinessCards.SingleOrDefault(x => x.Id == cardID);
            if (card.Rating < 2000000)
            {
                card.Rating += rate;
                context.SaveChanges();
            }
        }

        public ICollection<BusinessCardToField> GetAllBusinessCardToFieldsLinks(int cardID)
        {
            var links = from a in context.BusinessCardsToFields
                        where a.BusinessCardId == cardID
                        select a;
            return links.ToList();
        }

        public Field GetField(int fieldID)
        {
            return context.Fields.SingleOrDefault(x => x.Id == fieldID);
        }

        public ICollection<Field> GetAllProfileFields(string username)
        {
            return context.Profile.SingleOrDefault(x => x.UserName == username).Fields;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}