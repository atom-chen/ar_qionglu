//
//  MobPushUnity3DBridge.m
//  MobPush
//
//  Created by LeeJay on 2018/5/10.
//  Copyright © 2018年 com.mob. All rights reserved.
//

#import "MobPushUnity3DBridge.h"
#import <MobPush/MobPush.h>
#import <MOBFoundation/MOBFJson.h>
#import <MobPush/MPushNotificationConfiguration.h>
#import <MobPush/MobPush+Test.h>
#import <MobPush/MPushMessage.h>
#import "MobPushUnityCallback.h"

#if defined (__cplusplus)
extern "C" {
#endif
    
    extern void __iosMobPushSetAPNsForProduction (bool iosPro);
    
    extern void __iosMobAddPushReceiver (void *observer);
    
    extern void __iosMobPushSetupNotification (void *notification);

    extern void __iosMobPushAddLocalNotification (void *message);
    
    extern void __iosMobPushGetTags (void *observer);
    
    extern void __iosMobPushAddTags (void *tags, void *observer);
    
    extern void __iosMobPushDeleteTags (void *tags, void *observer);
    
    extern void __iosMobPushCleanAllTags (void *observer);
    
    extern void __iosMobPushGetAlias (void *observer);
    
    extern void __iosMobPushSetAlias (void *alias, void *observer);

    extern void __iosMobPushDeleteAlias (void *observer);

    extern void __iosMobPushGetRegistrationID (void *observer);
    
    extern void __iosMobPushSendMessage (int type, void *content, int space, void *extras, void *observer);
    
    MPushNotificationConfiguration *__parseNotiConfigHashtable (void *notificationInfo);
    MPushMessage *__parseMessageHashtable (void *messageInfo);
    
    void __iosMobPushSetAPNsForProduction (bool iosPro)
    {
        [MobPushUnityCallback defaultCallBack].isPro = iosPro == true ? YES : NO;
    }
    
    void __iosMobAddPushReceiver(void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        [[MobPushUnityCallback defaultCallBack] addPushObserver:observerStr];
    }
    
    void __iosMobPushSetupNotification (void *notification)
    {
        MPushNotificationConfiguration *config = __parseNotiConfigHashtable(notification);
        [MobPush setupNotification:config];
    }
    
    extern void __iosMobPushAddLocalNotification (void *messageInfo)
    {
        MPushMessage *message = __parseMessageHashtable(messageInfo);
        [MobPush addLocalNotification:message];
    }
    
    extern void __iosMobPushGetTags (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush getTagsWithResult:^(NSArray *tags, NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 3 ，操作 tag
            [resultDict setObject:@3 forKey:@"action"];
            // operation = 0 获取
            [resultDict setObject:@0 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                if (tags.count)
                {
                    NSString *tagStr = [tags componentsJoinedByString:@","];
                    
                    [resultDict setObject:tagStr forKey:@"tags"];
                }
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
        }];
    }
    
    extern void __iosMobPushAddTags (void *tags, void *observer)
    {
        NSString *theParamsStr = [NSString stringWithCString:tags encoding:NSUTF8StringEncoding];
        NSArray *tagParams = nil;
        
        if (theParamsStr)
        {
            tagParams = [theParamsStr componentsSeparatedByString:@","];
        }
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        [MobPush addTags:tagParams result:^(NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 3 ，操作 tag
            [resultDict setObject:@3 forKey:@"action"];
            // operation = 1 设置
            [resultDict setObject:@1 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
            
        }];
    }
    
    extern void __iosMobPushDeleteTags (void *tags, void *observer)
    {
        NSString *theParamsStr = [NSString stringWithCString:tags encoding:NSUTF8StringEncoding];
        NSArray *tagParams = nil;
        
        if (theParamsStr)
        {
            tagParams = [theParamsStr componentsSeparatedByString:@","];
        }
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush deleteTags:tagParams result:^(NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 3 ，操作 tag
            [resultDict setObject:@3 forKey:@"action"];
            // operation = 2 删除
            [resultDict setObject:@2 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
            
        }];
    }
    
    extern void __iosMobPushCleanAllTags (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush cleanAllTags:^(NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 3 ，操作 tag
            [resultDict setObject:@3 forKey:@"action"];
            // operation = 3 清空
            [resultDict setObject:@3 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
        }];
    }
    
    extern void __iosMobPushGetAlias (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush getAliasWithResult:^(NSString *alias, NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 4 ，操作 alias
            [resultDict setObject:@4 forKey:@"action"];
            // operation = 0 获取
            [resultDict setObject:@0 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                if (alias)
                {
                    [resultDict setObject:alias forKey:@"alias"];
                }
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
        }];
    }
    
    extern void __iosMobPushSetAlias (void *alias, void *observer)
    {
        NSString *aliasParam = [NSString stringWithCString:alias encoding:NSUTF8StringEncoding];
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush setAlias:aliasParam result:^(NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 4 ，操作 alias
            [resultDict setObject:@4 forKey:@"action"];
            // operation = 1 设置
            [resultDict setObject:@1 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
        }];
    }
    
    extern void __iosMobPushDeleteAlias (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush deleteAlias:^(NSError *error) {
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
            // action = 4 ，操作 alias
            [resultDict setObject:@4 forKey:@"action"];
            // operation = 2 删除
            [resultDict setObject:@2 forKey:@"operation"];
            if (error)
            {
                [resultDict setObject:@(error.code) forKey:@"errorCode"];
            }
            else
            {
                [resultDict setObject:@(0) forKey:@"errorCode"];
            }
            // 转成 json 字符串
            NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
            UnitySendMessage([observerStr UTF8String], "_MobPushCallback", [resultStr UTF8String]);
        }];
    }
    
    extern void __iosMobPushGetRegistrationID (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush getRegistrationID:^(NSString *registrationID, NSError *error) {
            if (registrationID)
            {
                UnitySendMessage([observerStr UTF8String], "_MobPushRegIdCallback", [registrationID UTF8String]);
            }
        }];
    }

    extern void __iosMobPushSendMessage (int type, void *content, int space, void *extras, void *observer)
    {
        NSString *contentParam = [NSString stringWithCString:content encoding:NSUTF8StringEncoding];

        NSDictionary *extrasDict = nil;
        if (extras)
        {
            NSString *theParam = [NSString stringWithCString:extras encoding:NSUTF8StringEncoding];
            extrasDict = [MOBFJson objectFromJSONString:theParam];
        }
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [MobPush sendMessageWithMessageType:type
                                    content:contentParam
                                      space:@(space)
                    isProductionEnvironment:[MobPushUnityCallback defaultCallBack].isPro
                                     extras:extrasDict
                                 linkScheme:@""
                                   linkData:@""
                                     result:^(NSError *error) {
                                         
                                         NSMutableDictionary *resultDict = [NSMutableDictionary dictionary];
                                        
                                         if (error)
                                         {
                                             [resultDict setObject:@(0) forKey:@"action"];
                                         }
                                         else
                                         {
                                             [resultDict setObject:@(1) forKey:@"action"];
                                         }
                                         // 转成 json 字符串
                                         NSString *resultStr = [MOBFJson jsonStringFromObject:resultDict];
                                         UnitySendMessage([observerStr UTF8String], "_MobPushDemoCallback", [resultStr UTF8String]);
                                         
                                     }];
    }
    
    MPushNotificationConfiguration *__parseNotiConfigHashtable (void *notificationInfo)
    {
        NSString *theParamsStr = [NSString stringWithCString:notificationInfo encoding:NSUTF8StringEncoding];
        NSDictionary *eventParams = [MOBFJson objectFromJSONString:theParamsStr];
        
        MPushNotificationConfiguration *config = [[MPushNotificationConfiguration alloc] init];
        config.types = (MPushAuthorizationOptions)[eventParams[@"type"] integerValue];
        return config;
    }

    MPushMessage *__parseMessageHashtable (void *messageInfo)
    {
        NSString *theParamsStr = [NSString stringWithCString:messageInfo encoding:NSUTF8StringEncoding];
        NSDictionary *eventParams = [MOBFJson objectFromJSONString:theParamsStr];
        
        MPushMessage *message = [[MPushMessage alloc] init];
        message.messageType = MPushMessageTypeLocal;
        MPushNotification *noti = [[MPushNotification alloc] init];
        
        noti.title = eventParams[@"title"];
        noti.body = eventParams[@"content"];
        noti.sound = eventParams[@"sound"];
        noti.badge = [eventParams[@"badge"] integerValue];
        noti.subTitle = eventParams[@"subTitle"];
        
        long timeStamp = [eventParams[@"timeStamp"] longValue];
        if (timeStamp == 0)
        {
            message.isInstantMessage = YES;
        }
        else
        {
            NSDate *currentDate = [NSDate dateWithTimeIntervalSinceNow:0];
            NSTimeInterval nowtime = [currentDate timeIntervalSince1970] * 1000;
            message.taskDate = nowtime + (NSTimeInterval)timeStamp;
        }
        
        message.notification = noti;
        
        return message;
    }
    
#if defined (__cplusplus)
}
#endif

@implementation MobPushUnity3DBridge

@end
