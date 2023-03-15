using AutoMapper;
using OnlineShop.ViewModels;
using OnlineShop.Model.Models;

namespace OnlineShop
{
    public class AutoMap
    {
        private static AutoMap? _instance = null;
        //safe-thread
        private static readonly object _lock = new object();
        public static AutoMap? Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AutoMap();
                        }
                    }
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        public MapperConfiguration MapperConfig
        {
            get { return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AboutVM, About>().ReverseMap();
                cfg.CreateMap<ApplicationGroupVM, ApplicationGroup>().ReverseMap();
                cfg.CreateMap<ApplicationRoleVM, ApplicationRole>().ReverseMap();
                cfg.CreateMap<ApplicationUserVM, ApplicationUser>().ReverseMap();
                cfg.CreateMap<ContactDetailVM, ContactDetail>().ReverseMap();
                cfg.CreateMap<ContactVM, Contact>().ReverseMap();
                cfg.CreateMap<ErrorsVM, Error>().ReverseMap();
                cfg.CreateMap<FeedbackVM, Feedback>().ReverseMap();
                cfg.CreateMap<FooterVM, Footer>().ReverseMap();
                cfg.CreateMap<MenuGroupsVM, MenuGroup>().ReverseMap();
                cfg.CreateMap<MenuTypeVM, MenuType>().ReverseMap();
                cfg.CreateMap<MenuVM, Menu>().ReverseMap();
                cfg.CreateMap<OrderDetailVM, OrderDetail>().ReverseMap();
                cfg.CreateMap<OrderVM, Order>().ReverseMap();
                cfg.CreateMap<PostCategoryVM, PostCategory>().ReverseMap();
                cfg.CreateMap<PostContentVM, PostContent>().ReverseMap();
                cfg.CreateMap<PostTagVM, PostTag>().ReverseMap();
                cfg.CreateMap<ProductCategoryVM, ProductCategory>().ReverseMap();
                cfg.CreateMap<ProductTagsVM, ProductTags>().ReverseMap();
                cfg.CreateMap<ProductVM, Product>().ReverseMap();
                cfg.CreateMap<SlideVM, Slide>().ReverseMap();
                cfg.CreateMap<SupportOnlineVM, SupportOnline>().ReverseMap();
                cfg.CreateMap<SystemConfigVM, SystemConfig>().ReverseMap();
                cfg.CreateMap<TagVM, Tag>().ReverseMap();
                cfg.CreateMap<VisitorStatisticVM, VisitorStatistic>().ReverseMap();

                //// sample
                //cfg.CreateMap<ReportHeaderViewModel, ReportResultModel>()
                //    .ForMember(dest => dest.create_at, act => act.MapFrom(src => DateTimeExtensions.SetKindUtc(DateTime.Parse(src.create_at!))))
                //    .ForMember(dest => dest.update_at, act => act.MapFrom(src => DateTimeExtensions.SetKindUtc(DateTime.Parse(src.update_at!))));
                //cfg.CreateMap<ReportResultModel, ReportHeaderViewModel>()
                //    .ForMember(dest => dest.create_at, act => act.MapFrom(src => src.create_at.ToString()))
                //    .ForMember(dest => dest.update_at, act => act.MapFrom(src => src.update_at.ToString()));

            });
            }
        }

        // how to use: var reportComment = AutoMap.Instance.Mapper.Map<ReportCommentViewModel>(curRecord);
        public Mapper Mapper
        {
            get { return new Mapper(AutoMap.Instance!.MapperConfig); }
        }
    }
}
